using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Stripe.Checkout;

namespace Mercure.API.Controllers;

[Route("/order")]
public class OrderController : ApiNoSecurityController
{
    private readonly MercureContext _context;
    private readonly IDistributedCache _distributedCache;

    public OrderController(MercureContext context, IDistributedCache distributedCache)
    {
        _context = context;
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// Route to buy what's inside cart
    /// </summary>
    /// <param name="randomId">need to give a randomId if not connected</param>
    /// <returns></returns>
    [HttpPost("buy")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status303SeeOther)]
    public async Task<IActionResult> BuyAction(string randomId)
    {
        var userContext = (User) HttpContext.Items["User"];

        var isAuthenticated = userContext != null;
        var hasRandomId = !string.IsNullOrEmpty(randomId);

        if (isAuthenticated)
        {
            // If the user is authenticated and is an admin he can't buy products
            // to limit the risk of buying products with the admin account
            var authRole = userContext.Role;
            if (RoleChecker.HasRole(authRole, RoleEnum.Admin))
            {
                return Unauthorized(new ErrorMessage("You are an admin, you can't buy products.",
                    StatusCodes.Status403Forbidden));
            }
        }

        // If the user is not authenticated and don't provide an id
        if (!hasRandomId && !isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You are not authenticated and don't provide an id.",
                StatusCodes.Status401Unauthorized));
        }

        // If the user is authenticated and provide an id
        if (hasRandomId && isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You can't get the cart of an authenticated user and providing an id.",
                StatusCodes.Status401Unauthorized));
        }

        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        var redisCart = await _distributedCache.GetStringAsync(cacheKey);
        var products = redisCart != null ? JsonConvert.DeserializeObject<Cart>(redisCart) : null;

        if (products == null || products.Products.Count == 0)
        {
            await _distributedCache.RemoveAsync(cacheKey);
            return BadRequest(new ErrorMessage("The cart is empty.", StatusCodes.Status400BadRequest));
        }

        var orderProducts = new List<OrderProduct>();
        products.Products.ForEach((p) =>
        {
            var productDb = _context.Products.FirstOrDefault(pr => pr.ProductId == p.Product.ProductId);
            if (productDb == null)
            {
                Logger.LogError("The product : " + p.Product.ProductId + ", was not found.");
                return;
            }

            var orderProduct = new OrderProduct()
            {
                Product = productDb,
                Quantity = p.Quantity,
                Shipped = false
            };

            orderProducts.Add(orderProduct);
        });

        if (orderProducts.Count == 0)
        {
            await _distributedCache.RemoveAsync(cacheKey);
            Logger.LogWarn("Somebody tried to buy an empty cart.");
            return BadRequest(new ErrorMessage("The cart is empty.", StatusCodes.Status400BadRequest));
        }

        const string domain = "http://localhost:5000";
        var options = new SessionCreateOptions()
        {
            Metadata = new Dictionary<string, string>()
            {
                {"user_id", isAuthenticated ? userContext.UserId.ToString() : null},
                {"random_id", hasRandomId ? randomId : null}
            },
            PaymentMethodTypes = new List<string>()
            {
                "card",
            },
            LineItems = GenerateListItemOptions(orderProducts),
            Mode = "payment",
            AllowPromotionCodes = false,
            Locale = "auto",
            SuccessUrl = domain + "/order/success?sessionId={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "/order/cancel?sessionId={CHECKOUT_SESSION_ID}",
            AutomaticTax = new SessionAutomaticTaxOptions()
            {
                Enabled = true
            },
            CustomerEmail = isAuthenticated ? userContext.Email : null
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        var order = new Order()
        {
            UserId = isAuthenticated ? userContext.UserId : null,
            OrderPrice = PredicatedTotalPrice(products.Products),
            OrderTaxPrice = 0,
            OrderDeliveryPrice = 0,
            OrderDate = session.Created,
            OrderStatus = false,
            SessionId = session.Id,
            Products = new List<OrderProduct>()
        };

        order.Products = orderProducts;
        
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        Logger.LogInfo("The order : " + order.OrderId + ", has been created. \n\nSession id : " + session.Id + ".");

        // Retourne l'url qui permet de payer
        return Ok(session.Url);
    }

    /// <summary>
    /// Route to buy an canceled order
    /// </summary>
    /// <param name="orderId">order id that has been canceled</param>
    /// <returns></returns>
    [HttpPost("buy-again")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> BuyAgainAction(string orderId)
    {
        if (string.IsNullOrEmpty(orderId))
        {
            return BadRequest(new ErrorMessage("You need to provide an order id.",
                StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            Logger.LogWarn("Somebody tried to buy again an order without being authenticated. Order id : " + orderId +
                           ".");
            return BadRequest(new ErrorMessage("You need to be authenticated to order again an order.",
                StatusCodes.Status400BadRequest));
        }

        bool isOrderIdParsed = int.TryParse(orderId, out int orderIdParsed);
        if (!isOrderIdParsed)
        {
            return BadRequest(new ErrorMessage("Your product id is not an number", StatusCodes.Status400BadRequest));
        }

        var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderIdParsed);
        if (order == null)
        {
            Logger.LogWarn("Somebody tried to buy again an order that doesn't exist. Order id : " + orderId + ".");
            return BadRequest(new ErrorMessage("The order doesn't exist.",
                StatusCodes.Status400BadRequest));
        }

        var orderProduct = _context.OrderProducts
            .Include(o => o.Product)
            .Where(o => o.OrderId == orderIdParsed);

        if (!orderProduct.Any())
        {
            Logger.LogWarn("Somebody tried to buy again an order that doesn't have any product. Order id : " + orderId +
                           ".");
            return BadRequest(new ErrorMessage("The order doesn't have any product.",
                StatusCodes.Status400BadRequest));
        }

        var orderProducts = new List<OrderProduct>();
        orderProduct.ToList().ForEach((p) =>
        {
            var productDb = _context.Products.FirstOrDefault(pr => pr.ProductId == p.Product.ProductId);
            if (productDb == null)
            {
                Logger.LogError("The product : " + p.Product.ProductId + ", was not found.");
            }
            else
            {
                var product = new OrderProduct()
                {
                    Product = productDb,
                    Quantity = p.Quantity,
                    Shipped = false
                };
                orderProducts.Add(product);
            }
        });

        if (orderProducts.Count == 0)
        {
            Logger.LogWarn("Somebody tried to buy an empty cart.");
            return BadRequest(new ErrorMessage("The cart is empty.", StatusCodes.Status400BadRequest));
        }

        const string domain = "http://localhost:5000";
        var options = new SessionCreateOptions()
        {
            Metadata = new Dictionary<string, string>()
            {
                {"user_id", userContext.UserId.ToString()},
            },
            PaymentMethodTypes = new List<string>()
            {
                "card",
            },
            LineItems = GenerateListItemOptions(orderProducts),
            Mode = "payment",
            AllowPromotionCodes = false,
            Locale = "auto",
            SuccessUrl = domain + "/order/success?sessionId={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "/order/cancel?sessionId={CHECKOUT_SESSION_ID}",
            AutomaticTax = new SessionAutomaticTaxOptions()
            {
                Enabled = true
            },
            CustomerEmail = userContext.Email
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        order.SessionId = session.Id;
        await _context.SaveChangesAsync();

        Logger.LogInfo("The order : " + order.OrderId + ", has been created to order again. \n\nSession id : " +
                       session.Id + ".");

        // Retourne l'url qui permet de pouvoir payer les articles
        return Ok(session.Url);
    }

    /// <summary>
    /// Route to finish order
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet("success")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> SuccessAction(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest(new ErrorMessage("The session id is empty.", StatusCodes.Status400BadRequest));
        }

        var sessionOptions = new SessionGetOptions();
        sessionOptions.AddExpand("line_items");

        var service = new SessionService();
        var session = await service.GetAsync(sessionId, sessionOptions);

        if (session == null)
        {
            Logger.LogWarn("The sesssion : " + session.Id + ", was not found.");
            return BadRequest(new ErrorMessage("The session is empty.", StatusCodes.Status400BadRequest));
        }

        var order = _context.Orders.FirstOrDefault(o => o.SessionId == session.Id);
        if (order == null)
        {
            Logger.LogError("The order has nerver been created in the database but is valid.");
            return BadRequest(new ErrorMessage("The order has nerver been created in the database but is valid.",
                StatusCodes.Status400BadRequest));
        }

        var currency = session.Currency;
        var totalPrice = session.AmountTotal / 100;
        var taxPrice = 0;
        var deliveryPrice = 0;

        var paymentStatus = session.PaymentStatus;

        // We check if the payment is paid and if the order is not already paid
        if (paymentStatus != "paid" && !order.OrderStatus)
        {
            return BadRequest(new ErrorMessage("This payment is not paid.", StatusCodes.Status400BadRequest));
        }

        // We check if the payment is paid and if the order is already paid
        if (paymentStatus != "paid" && order.OrderStatus)
        {
            return BadRequest(new ErrorMessage("This payment is already paid.", StatusCodes.Status400BadRequest));
        }

        order.OrderPrice = totalPrice;
        order.OrderDeliveryPrice = deliveryPrice;
        order.OrderTaxPrice = taxPrice;
        order.OrderStatus = true;

        await _context.SaveChangesAsync();
        Logger.LogInfo("The order : " + order.OrderId + ", has been paid.");

        // Clear the cart
        var userContext = (User) HttpContext.Items["User"];
        var isAuthenticated = userContext != null;
        var randomId = session.Metadata["random_id"];

        if (isAuthenticated && randomId != null)
        {
            Logger.LogWarn("The user : " + userContext.UserId + ", has a random id in the session.");
            return BadRequest(new ErrorMessage("The user has a random id in the session.",
                StatusCodes.Status400BadRequest));
        }

        if (!isAuthenticated && randomId == null)
        {
            Logger.LogWarn("The user is not authenticated and the random id is empty.");
            return BadRequest(new ErrorMessage("The user is not authenticated and the random id is empty.",
                StatusCodes.Status400BadRequest));
        }

        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        await _distributedCache.RemoveAsync(cacheKey);

        Logger.LogInfo("The cart has been cleared for the order : " + order.OrderId + ".");

        return Redirect("http://localhost:4200/order/success?orderNumber=" + order.OrderId);
    }

    /// <summary>
    /// Cancel the order
    /// </summary>
    /// <param name="sessionId">the session to cancel</param>
    /// <returns></returns>
    [HttpGet("cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CancelAction(string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
        {
            return BadRequest(new ErrorMessage("The session id is empty.", StatusCodes.Status400BadRequest));
        }

        var sessionOptions = new SessionGetOptions();
        sessionOptions.AddExpand("line_items");

        var service = new SessionService();
        var session = await service.GetAsync(sessionId, sessionOptions);

        if (session == null)
        {
            Logger.LogWarn("The sesssion : " + session.Id + ", was not found.");
            return BadRequest(new ErrorMessage("The session is empty.", StatusCodes.Status400BadRequest));
        }

        var order = _context.Orders.FirstOrDefault(o => o.SessionId == session.Id);
        if (order == null)
        {
            Logger.LogError("The order has nerver been created in the database but is valid.");
            return BadRequest(new ErrorMessage("The order has nerver been created in the database but is valid.",
                StatusCodes.Status400BadRequest));
        }

        // Clear the cart
        var userContext = (User) HttpContext.Items["User"];

        var isAuthenticated = userContext != null;
        var randomId = session.Metadata["random_id"];

        if (isAuthenticated && randomId != null)
        {
            Logger.LogWarn("The user : " + userContext.UserId + ", has a random id in the session.");
            return BadRequest(new ErrorMessage("The user has a random id in the session.",
                StatusCodes.Status400BadRequest));
        }

        if (!isAuthenticated && randomId == null)
        {
            Logger.LogWarn("The user is not authenticated and the random id is empty.");
            return BadRequest(new ErrorMessage("The user is not authenticated and the random id is empty.",
                StatusCodes.Status400BadRequest));
        }

        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        await _distributedCache.RemoveAsync(cacheKey);

        Logger.LogInfo("The cart has been cleared for the order : " + order.OrderId +
                       ". Beacaue the payment has been canceled.");

        // We set the session id to null, to to be able to create a new order and know it was canceled
        order.SessionId = null;
        await _context.SaveChangesAsync();

        return Redirect("http://localhost:4200/order/cancel?orderNumber=" + order.OrderId);
    }

    /// <summary>
    /// Map the cart to a list of session line item options
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    private static List<SessionLineItemOptions> GenerateListItemOptions(List<OrderProduct> products)
    {
        var result = new List<SessionLineItemOptions>();
        products.ForEach(p =>
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (p.Product == null) throw new ArgumentNullException(nameof(p.Product));

            var sessionLineItem = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    Currency = "CHF",
                    UnitAmountDecimal = p.Product.ProductPrice * 100,
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = p.Product.ProductName,
                        Description = p.Product.ProductDescription,
                        Metadata = new Dictionary<string, string>()
                        {
                            {"product_id", p.Product.ProductId.ToString()},
                            {"product_price", p.Product.ProductPrice.ToString()},
                            {"product_quantity", p.Quantity.ToString()}
                        }
                    }
                },
                Quantity = p.Quantity
            };

            result.Add(sessionLineItem);
        });

        return result;
    }

    /// <summary>
    /// Get the order
    /// </summary>
    /// <param name="orderNumber"></param>
    /// <returns></returns>
    [HttpGet("{orderNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetOrder(string orderNumber)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return BadRequest(new ErrorMessage("The user is not authenticated.", StatusCodes.Status400BadRequest));
        }

        if (string.IsNullOrEmpty(orderNumber))
        {
            return BadRequest(new ErrorMessage("The order number is empty.", StatusCodes.Status400BadRequest));
        }

        var isOrderNumberParsed = int.TryParse(orderNumber, out var orderNumberParsed);
        if (!isOrderNumberParsed)
        {
            return BadRequest(new ErrorMessage("The order number is not a valid number.",
                StatusCodes.Status400BadRequest));
        }

        var order = await _context.Orders.Include(o => o.Products)
            .FirstOrDefaultAsync(o => o.OrderId == orderNumberParsed);
        if (order == null)
        {
            return NotFound(new ErrorMessage("No order found with this number.", StatusCodes.Status400BadRequest));
        }

        if (order.UserId != userContext.UserId)
        {
            return BadRequest(new ErrorMessage("The user is not the owner of the order.",
                StatusCodes.Status400BadRequest));
        }

        return Ok(new OrderDto(order));
    }

    /// <summary>
    /// Get the status of an order
    /// </summary>
    /// <param name="orderNumber"></param>
    /// <returns></returns>
    [HttpGet("status/{orderNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetStatusOrder(string orderNumber)
    {
        if (string.IsNullOrEmpty(orderNumber))
        {
            return BadRequest(new ErrorMessage("The order number is empty.", StatusCodes.Status400BadRequest));
        }

        var isOrderNumberParsed = int.TryParse(orderNumber, out var orderNumberParsed);
        if (!isOrderNumberParsed)
        {
            return BadRequest(new ErrorMessage("The order number is not a valid number.",
                StatusCodes.Status400BadRequest));
        }

        var order = await _context.Orders.Include(o => o.Products)
            .FirstOrDefaultAsync(o => o.OrderId == orderNumberParsed);
        if (order == null)
        {
            return NotFound(new ErrorMessage("No order found with this number.", StatusCodes.Status400BadRequest));
        }

        return Ok(order.OrderStatus);
    }

    /// <summary>
    /// Get the history of the user
    /// </summary>
    /// <returns></returns>
    [HttpGet("history")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetHistoryUser()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return BadRequest(new ErrorMessage("The user is not authenticated.", StatusCodes.Status400BadRequest));
        }

        var orders = _context.OrderProducts
            .Include(op => op.Product)
            .Include(op => op.Order)
            .Where(op => op.Order.UserId == userContext.UserId)
            .ToList();

        var ordersDto = new List<OrderDto>();
        orders.ForEach(o => ordersDto.Add(new OrderDto(orders, o.Order)));
        
        // BUG: Duplique N fois selon le nombre de produit => fix rapide distinct => pas très propre mais fonctionnel
        ordersDto = ordersDto.DistinctBy(o => o.OrderNumber).ToList();

        if (ordersDto.Count == 0)
        {
            return NotFound(new ErrorMessage("No order found for this user.", StatusCodes.Status404NotFound));
        }
        
        return Ok(ordersDto);
    }

    /// <summary>
    /// Predict the total price of the cart (can be different from the real price because of the taxes or the delivery price)
    /// </summary>
    /// <param name="products"></param>
    /// <returns></returns>
    private static int PredicatedTotalPrice(List<CartProduct> products)
    {
        // ReSharper disable once PossibleInvalidOperationException
        return (int) products.Sum(p => p.Product.ProductPrice * p.Quantity);
    }

    /// <summary>
    /// Convert the order product to cart product
    /// </summary>
    /// <param name="orderProducts"></param>
    /// <returns></returns>
    private List<CartProduct> OrderProductToCartProduct(List<OrderProduct> orderProducts)
    {
        var result = new List<CartProduct>();
        orderProducts.ForEach(p =>
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            if (p.Product == null) throw new ArgumentNullException(nameof(p.Product));

            var cartProduct = new CartProduct()
            {
                Product = p.Product.ToDto(),
                Quantity = p.Quantity
            };
            result.Add(cartProduct);
        });

        return result;
    }
}