using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mercure.API.Controllers;

/// <summary>
/// Api controller for shopping, no need to be authenticated
/// </summary>
[Route("shopping")]
public class ShoppingController : ApiNoSecurityController
{
    private readonly MercureContext _context;

    public ShoppingController(MercureContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get the best seller products
    /// </summary>
    /// <returns></returns>
    [HttpGet("bestSeller")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderProduct>))]
    public async Task<IActionResult> GetBestSeller()
    {
        // TODO: Faire des tests
        var bestSeller = _context.OrderProducts
            .Include(o => o.Product)
            .Include(o => o.Order)
            .Where(o => o.Order.OrderDate.Date.AddDays(5) < DateTime.Now)
            .OrderBy(o => o.Order.OrderDate)
            .GroupBy(o => o.ProductId)
            .OrderBy(g  => g.Count())
            .Take(10);

        if (!bestSeller.Any())
        {
            Logger.LogError("No best seller found, that should not happen or the database is empty");
            return NotFound(new ErrorMessage("No best seller found", StatusCodes.Status404NotFound));
        }

        return Ok(bestSeller);
    }
    
    /// <summary>
    /// Get the brands of the products
    /// </summary>
    /// <returns></returns>
    [HttpGet("brands")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetBrands()
    {
        var productBrand = _context.Products.Select(p => p.ProductBrandName).Distinct();
        if (!productBrand.Any())
        {
            return NotFound(new ErrorMessage("No brands found", StatusCodes.Status404NotFound));
        }
        
        return Ok(productBrand);
    }
    
    /// <summary>
    /// Get the categories of the products
    /// </summary>
    /// <returns></returns>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories.ToListAsync();
        if (!categories.Any())
        {
            return NotFound(new ErrorMessage("No categories found", StatusCodes.Status404NotFound));
        }

        return Ok(categories);
    }

    /// <summary>
    /// Get result from a search
    /// </summary>
    /// <param name="search"></param>
    /// <param name="brand"></param>
    /// <param name="category"></param>
    /// <param name="minPrice"></param>
    /// <param name="maxPrice"></param>
    /// <returns></returns>
    [HttpGet("search/{search}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public IActionResult Search(string search, string brand, string category, string minPrice, string maxPrice)
    {
        var products = _context.Products.Where(p => 
            p.ProductName.ToLower().Contains(search.ToLower()) || 
            p.ProductDescription.ToLower().Contains(search.ToLower()) || 
            p.ProductBrandName.ToLower().Contains(search.ToLower()));
        
        if (!string.IsNullOrEmpty(brand))
        {
            products = products.Where(p => p.ProductBrandName.ToLower() == brand.ToLower());
        }
        
        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Categories.Any(c => c.CategoryTitle.ToLower() == category.ToLower()));
        }
        
        if (!string.IsNullOrEmpty(minPrice))
        {
            bool isParsed = int.TryParse(minPrice, out int minPriceParsed);
            if (!isParsed)
            {
                return BadRequest(new ErrorMessage("Min price is not a number", StatusCodes.Status400BadRequest));    
            }
            
            products = products.Where(p => p.ProductPrice >= minPriceParsed);
        }
        
        if (!string.IsNullOrEmpty(maxPrice))
        {
            bool isParsed = int.TryParse(maxPrice, out int maxPriceParsed);
            if (!isParsed)
            {
                return BadRequest(new ErrorMessage("Max price is not a number", StatusCodes.Status400BadRequest));    
            }
            
            products = products.Where(p => p.ProductPrice <= maxPriceParsed);
        }
        
        if (products.Any())
        {
            return Ok(products.Take(30).Include(p => p.Categories));
        }
        
        return NotFound(new ErrorMessage("No product found.", StatusCodes.Status404NotFound));
    }
}