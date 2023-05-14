using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    /// Get random products for the home pageIndex
    /// </summary>
    /// <returns></returns>
    [HttpGet("home")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationProduct))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetProductsWithPagination(string pageIndex = "1", string pageSize = "15")
    {
        var isPageParsed = int.TryParse(pageIndex, out var pageParsed);
        if (!isPageParsed)
        {
            return BadRequest(new ErrorMessage("Page number is not valid", StatusCodes.Status400BadRequest));
        }

        var isPageSizeParsed = int.TryParse(pageSize, out var pageSizeParsed);
        if (!isPageSizeParsed)
        {
            return BadRequest(new ErrorMessage("Page size is not valid", StatusCodes.Status400BadRequest));
        }

        if (!(pageSizeParsed > 5 && pageSizeParsed < 25))
        {
            return BadRequest(new ErrorMessage("Page size is not valid, please be between 5 and 25.",
                StatusCodes.Status400BadRequest));
        }

        var products = await _context.Products
            .OrderByDescending(p => p.ProductCreationDate)
            .Skip((pageParsed > 0 ? pageParsed : 0) * pageSizeParsed)
            .Take(pageSizeParsed)
            .Include(p => p.Categories)
            .Select(p => new ProductDto(p, true))
            .ToListAsync();

        var totalProducts = await _context.Products.CountAsync();
        var totalPages = (int) Math.Ceiling((double) totalProducts / pageSizeParsed);

        // Permet de ne pas avoir de page en trop
        if (totalPages > 0)
        {
            totalPages -= 1;
        }

        var paginationProduct = new PaginationProduct(products, pageParsed, pageSizeParsed, totalPages, totalProducts);

        return Ok(paginationProduct);
    }

    /// <summary>
    /// Get random products for the home pageIndex
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .OrderBy(p => Guid.NewGuid())
            .Take(4)
            .Include(p => p.Categories)
            .Select(p => new ProductDto(p, true))
            .ToListAsync();

        if (!products.Any())
        {
            Logger.LogError("No products found, that should not happen or the database is empty");
            return NotFound(new ErrorMessage("No products found", StatusCodes.Status404NotFound));
        }

        return Ok(products);
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
        /*
            Get the best seller products
            Get the orders of the last 5 days
            Group by product id
            Order by descending
            Take the 10 first
        */
        var bestSeller = _context.OrderProducts
            .Include(o => o.Order)
            .Where(o => o.Order.OrderDate.Date.AddDays(5) > DateTime.Now)
            .GroupBy(op => op.ProductId)
            .Select(x => new {x.Key, Count = x.Count()})
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToList();

        if (!bestSeller.Any())
        {
            Logger.LogError("No best seller found, that should not happen or the database is empty");
            return NotFound(new ErrorMessage("No best seller found", StatusCodes.Status404NotFound));
        }

        var bestSellerProducts = new List<ProductDto>();
        foreach (var bestSellerProduct in bestSeller)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Stock)
                .FirstOrDefaultAsync(p => p.ProductId == bestSellerProduct.Key);

            if (product != null)
            {
                bestSellerProducts.Add(new ProductDto(product, true));
            }
        }

        return Ok(bestSellerProducts);
    }

    /// <summary>
    /// Autocomplete for the search bar
    /// </summary>
    /// <param name="value"></param>
    [HttpGet("autocomplete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> Autocomplete(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return BadRequest(new ErrorMessage("Value is empty", StatusCodes.Status400BadRequest));
        }

        var produtsName = _context.Products.Where(p =>
                p.ProductName.ToLower().Contains(value.ToLower()) ||
                p.ProductDescription.ToLower().Contains(value.ToLower()) ||
                p.ProductBrandName.ToLower().Contains(value.ToLower()))
            .Select(p => p.ProductName)
            .Distinct()
            .Take(10)
            .ToList();

        if (!produtsName.Any())
        {
            return NotFound(new ErrorMessage("No products found", StatusCodes.Status404NotFound));
        }

        return Ok(produtsName);
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
        var categories = await _context.Categories.Select(c => c.CategoryTitle).Distinct().ToListAsync();
        if (!categories.Any())
        {
            return NotFound(new ErrorMessage("No categories found", StatusCodes.Status404NotFound));
        }

        return Ok(categories);
    }

    /// <summary>
    /// Get result from a search
    /// </summary>
    /// <param name="search">The name that can help us find the product</param>
    /// <param name="brand">Select a brand</param>
    /// <param name="category">Select a category</param>
    /// <param name="minPrice">The minimum price of the searched product</param>
    /// <param name="maxPrice">The maximum price of the searched product</param>
    /// <param name="pageIndex">The page index</param>
    /// <param name="pageSize">Element shown per page</param>
    /// <returns></returns>
    [HttpGet("search/{search}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationProduct))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> Search(string search, string brand, string category, string minPrice,
        string maxPrice, string pageIndex = "1", string pageSize = "15")
    {
        if (string.IsNullOrEmpty(search))
        {
            return BadRequest(new ErrorMessage("Search is empty", StatusCodes.Status400BadRequest));
        }

        var isPageParsed = int.TryParse(pageIndex, out var pageParsed);
        if (!isPageParsed)
        {
            return BadRequest(new ErrorMessage("Page number is not valid", StatusCodes.Status400BadRequest));
        }

        var isPageSizeParsed = int.TryParse(pageSize, out var pageSizeParsed);
        if (!isPageSizeParsed)
        {
            return BadRequest(new ErrorMessage("Page size is not valid", StatusCodes.Status400BadRequest));
        }

        // Trim the search param
        search = search.Trim();

        var products = _context.Products
            .Where(p =>
                p.ProductName.ToLower().Contains(search.ToLower()) ||
                p.ProductDescription.ToLower().Contains(search.ToLower()) ||
                p.ProductBrandName.ToLower().Contains(search.ToLower()));

        if (!string.IsNullOrEmpty(brand))
        {
            var splitBrand = brand.ToLower().Split(",");
            
            // TODO: Si possible normalisé dans le front et dans la requête sql
            // Pour l'instant on passe direct le nom en entier en esperant que la requête web ne casse rien.
            products = products.Where(p =>  splitBrand.Contains(p.ProductBrandName.ToLower()));
        }

        if (!string.IsNullOrEmpty(category))
        {
            var splitCategory = category.ToLower().Split(",");
            products = products.Where(p => p.Categories.Any(c => splitCategory.Contains(c.CategoryTitle.ToLower())));
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
            var responseProduct = products
                .Skip((pageParsed > 0 ? pageParsed : 0) * pageSizeParsed)
                .Take(pageSizeParsed)
                .Include(p => p.Categories)
                .Select(p => new ProductDto(p, true))
                .ToList();

            var totalProducts = await products.CountAsync();
            var totalPages = (int) Math.Ceiling((double) totalProducts / pageSizeParsed);
            var paginationProduct =
                new PaginationProduct(responseProduct, pageParsed, pageSizeParsed, totalPages, totalProducts);

            return Ok(paginationProduct);
        }

        return NotFound(new ErrorMessage("No product found.", StatusCodes.Status404NotFound));
    }
}