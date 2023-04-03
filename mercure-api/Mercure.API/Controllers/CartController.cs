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
public class CartController : ApiSecurityController
{
    
    private readonly IMemoryCache _cache;
    private readonly MercureContext _context;
    private readonly IDistributedCache _distributedCache;

    public CartController(IMemoryCache cache, MercureContext context, IDistributedCache distributedCache)
    {
        _cache = cache;
        _context = context;
        _distributedCache = distributedCache;
    }

    /// <summary>
    /// Get the cart of the user
    /// </summary>
    /// <returns></returns>
    [HttpGet("/")]
    public IActionResult GetCart()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        var cacheKey = $"cart-{userContext.UserId}";
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
    /// <returns></returns>
    [HttpPost("/add/{productId}")]
    public async Task<IActionResult> AddProductCart(string productId)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
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
        
        var cacheKey = $"cart-{userContext.UserId}";
        var redisCart = _distributedCache.GetString(cacheKey);
        if (redisCart != null)
        {
            var cart = JsonConvert.DeserializeObject<Cart>(redisCart);
            cart.AddProduct(product);
            
            _distributedCache.SetString(cacheKey, JsonConvert.SerializeObject(cart));
            
            var cartProductAdded = cart.Products.Find(p => p.Product.ProductId == productIdParsed);
            return Ok(cartProductAdded);
        }
        else
        {
            var cart = new Cart(userContext.UserId);
            cart.AddProduct(product);
            
            _distributedCache.SetString(cacheKey, JsonConvert.SerializeObject(cart));
            
            var cartProductAdded = cart.Products.Find(p => p.Product.ProductId == productIdParsed);
            return Ok(cartProductAdded);
        }
    }

    /// <summary>
    /// Remove a product from the cart
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete("/remove/{productId}")]
    public async Task<IActionResult> RemoveProductCart(string productId)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
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
        
        var cacheKey = $"cart-{userContext.UserId}";
        var redisCart = _distributedCache.GetString(cacheKey);
        if (redisCart != null)
        {
            var cart = JsonConvert.DeserializeObject<Cart>(redisCart);
            cart.RemoveProduct(product);
            
            _distributedCache.SetString(cacheKey, JsonConvert.SerializeObject(cart));
            return Ok(new ErrorMessage("Product removed from cart", StatusCodes.Status200OK));
        }
        
        return NotFound(new ErrorMessage("Cart not found", StatusCodes.Status404NotFound));
    }
}