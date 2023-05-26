using System.Collections.Generic;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Mercure.API.Controllers;

/// <summary>
/// Controller for the cart only for authenticated users
/// </summary>
[Route("cart")]
public class CartController : ApiNoSecurityController
{
    
    private readonly MercureContext _context;
    private readonly IDistributedCache _distributedCache;

    public CartController(MercureContext context, IDistributedCache distributedCache)
    {
        _context = context;
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// Get the cart of the user
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Cart))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult GetCart(string randomId)
    {
        var userContext = (User) HttpContext.Items["User"];
        var isAuthenticated = userContext != null;

        var hasRandomId = !string.IsNullOrEmpty(randomId);

        // If the user is not authenticated and don't provide an id
        if (!hasRandomId && !isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You are not authenticated and don't provide an id.", StatusCodes.Status401Unauthorized));
        }

        // If the user is authenticated and provide an id
        if (hasRandomId && isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You can't get the cart of an authenticated user and providing an id.", StatusCodes.Status401Unauthorized));
        }
        
        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        var redisCart = _distributedCache.GetString(cacheKey);
        if (redisCart != null)
        {
            return Ok(JsonConvert.DeserializeObject<Cart>(redisCart));
        }

        return NotFound(new ErrorMessage("Cart not found", StatusCodes.Status404NotFound));
    }

    /// <summary>
    /// Add a product to the cart
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="randomId"></param>
    /// <param name="quantity"></param>
    /// <returns></returns>
    [HttpPost("add/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartProduct))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> AddProductCart(string productId, string randomId, string quantity = "1")
    {
        var userContext = (User) HttpContext.Items["User"];
        var isAuthenticated = userContext != null;

        var hasRandomId = !string.IsNullOrEmpty(randomId);
        
        // If the user is not authenticated and don't provide an id
        if (!hasRandomId && !isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You are not authenticated and don't provide an id.", StatusCodes.Status401Unauthorized));
        }

        // If the user is authenticated and provide an id
        if (hasRandomId && isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You can't add an product in the cart when beeing connected and providing an id.", StatusCodes.Status401Unauthorized));
        }
        
        var isProductParsed = int.TryParse(productId, out var productIdParsed);
        if (!isProductParsed)
        {
            return BadRequest(new ErrorMessage("Product id is not valid", StatusCodes.Status400BadRequest));
        }
        
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productIdParsed);
        if (product == null)
        {
            return NotFound(new ErrorMessage("Product not found", StatusCodes.Status404NotFound));
        }
        
        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        var redisCart = await _distributedCache.GetStringAsync(cacheKey);
        
        // Set the expiration of the cache to 1 day if the user is not authenticated and 30 days if the user is authenticated
        var cacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(System.TimeSpan.FromDays(isAuthenticated ? 30 : 1));
        
        bool isQuantityParsed = int.TryParse(quantity, out int parsedQuantity);
        if (!isQuantityParsed)
        {
            return BadRequest(new ErrorMessage("Quantity is not a number: " + quantity, StatusCodes.Status400BadRequest));
        }
        
        if (redisCart != null)
        {
            var cart = JsonConvert.DeserializeObject<Cart>(redisCart);
            cart.AddProduct(product, parsedQuantity);

            await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(cart), cacheOptions);
            
            var cartProductAdded = cart.Products.Find(p => p.Product.ProductId == productIdParsed);
            return Ok(cartProductAdded);
        }
        else
        {
            var cart = new Cart(isAuthenticated ? userContext.UserId.ToString() : randomId);
            cart.AddProduct(product, parsedQuantity);
            
            await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(cart), cacheOptions);
            
            var cartProductAdded = cart.Products.Find(p => p.Product.ProductId == productIdParsed);
            return Ok(cartProductAdded);
        }
    }

    /// <summary>
    /// Remove a product from the cart
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="randomId"></param>
    /// <returns></returns>
    [HttpDelete("remove/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> RemoveProductCart(string productId, string randomId)
    {
        var userContext = (User) HttpContext.Items["User"];
        var isAuthenticated = userContext != null;

        var hasRandomId = !string.IsNullOrEmpty(randomId);

        // If the user is not authenticated and don't provide an id
        if (!hasRandomId && !isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You are not authenticated and don't provide an id.", StatusCodes.Status401Unauthorized));
        }

        // If the user is authenticated and provide an id
        if (hasRandomId && isAuthenticated)
        {
            return BadRequest(new ErrorMessage("You can't add an product in the cart when beeing connected and providing an id.", StatusCodes.Status401Unauthorized));
        }
        
        var isProductParsed = int.TryParse(productId, out var productIdParsed);
        if (!isProductParsed)
        {
            return BadRequest(new ErrorMessage("Product id is not valid", StatusCodes.Status400BadRequest));
        }
        
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productIdParsed);
        if (product == null)
        {
            return NotFound(new ErrorMessage("Product not found", StatusCodes.Status404NotFound));
        }
        
        var cacheKey = isAuthenticated ? $"cart-{userContext.UserId}" : $"cart-{randomId}";
        var redisCart = await _distributedCache.GetStringAsync(cacheKey);
        if (redisCart != null)
        {
            var cart = JsonConvert.DeserializeObject<Cart>(redisCart);
            cart.RemoveProduct(product);
            
            await _distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(cart));
            return Ok(new ErrorMessage("Product removed from cart", StatusCodes.Status200OK));
        }
        
        return NotFound(new ErrorMessage("Cart not found", StatusCodes.Status404NotFound));
    }
}