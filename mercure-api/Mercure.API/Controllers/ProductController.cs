using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mercure.API.Controllers;

/// <summary>
/// All the routes for the products
/// </summary>
[Route("products")]
public class ProductController : ApiNoSecurityController
{

    private readonly MercureContext _context;

    public ProductController(MercureContext context)
    {
        _context = context;
    }

    
    /// <summary>
    /// Route for the management of the products to get them all.
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> ProductAll()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("You are not authenticated", StatusCodes.Status401Unauthorized));
        }

        var userRole = userContext.Role;
        if (!RoleChecker.HasRole(userRole, RoleEnum.Admin))
        {
            return Unauthorized(new ErrorMessage("You don't have the right to access this page.",
                StatusCodes.Status401Unauthorized));
        }

        // write log before the request because it can take a while and we want to keep track of it
        Logger.LogInfo("User (" + userContext.UserId + "/" + userContext.Role.RoleName +
                       ") is trying to get all products, this request can take a while.");

        var products = _context.Products
            .Select(p => new ProductDto(p, false))
            .ToList();
        

        if (!products.Any())
        {
            return NotFound(new ErrorMessage("No prod found.", StatusCodes.Status404NotFound));
        }

        // write log to keep track of the request
        Logger.LogInfo(
            "The request to get all products has been successfully completed. (" + products.Count + " products found)");

        return Ok(products);
    }

    /// <summary>
    /// Get the product for the given idq
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet("{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> ProductGet(string productId)
    {
        if (string.IsNullOrEmpty(productId))
        {
            return BadRequest(new ErrorMessage("Product Id is required", StatusCodes.Status400BadRequest));
        }
        
        bool isParsed = int.TryParse(productId, out int id);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Product Id is not a number", StatusCodes.Status400BadRequest));
        }
        
        var productDb = await _context.Products
            .Include(p => p.Stock)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.ProductId== id);
        
        if (productDb == null)
        {
            return NotFound(new ErrorMessage("Product not found", StatusCodes.Status404NotFound));
        }

        return Ok(new ProductDto(productDb, true));
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productName">The name of the product.</param>
    /// <param name="productBrandName">The name of the product's brand.</param>
    /// <param name="productDescription">The description of the product.</param>
    /// <param name="productPrice">The price of the product.</param>
    /// <param name="stockId">The identifier of the stock.</param>
    /// <param name="categories">The categories of the product.</param>
    /// <returns>Returns a ProductDto object that represents the created product.</returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> ProductCreate(
        [FromForm] string productName, 
        [FromForm] string productBrandName, 
        [FromForm] string productDescription,
        [FromForm] int productPrice,
        [FromForm] int stockId,
        [FromForm] string categories
        )
    {
        if (string.IsNullOrEmpty(productName) && productName.Length <= ConstantRules.MaxLengthName)
        {
            return BadRequest(new ErrorMessage("Your product need a name", StatusCodes.Status400BadRequest));
        }
        
        if (string.IsNullOrEmpty(productBrandName))
        {
            return BadRequest(new ErrorMessage("You need to give us a brand name", StatusCodes.Status400BadRequest));
        }
        
        if (string.IsNullOrEmpty(productDescription) && productDescription.Length <= ConstantRules.MaxLengthDescription)
        {
            return BadRequest(new ErrorMessage("You need a description for your product", StatusCodes.Status400BadRequest));
        }
        
        if (productPrice <= 0)
        {
            return BadRequest(new ErrorMessage("Price is required to create an product", StatusCodes.Status400BadRequest));
        }
        
        if (stockId <= 0)
        {
            return BadRequest(new ErrorMessage("You need to specify a stock", StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        if (!RoleChecker.HasSuperiorRole(userContext.Role, RoleEnum.ProductManager))
        {
            return Unauthorized(new ErrorMessage("You cannot create an product", StatusCodes.Status401Unauthorized));
        }
        
        var product = new Product(productBrandName, productName, productDescription, productPrice, DateTime.Now, DateTime.Now, stockId);
        product.Categories = new List<Category>();
        
        if (!string.IsNullOrEmpty(categories))
        {
            var categoriesList = categories.Split(",");
            
            foreach (var category in categoriesList)
            {
                bool isParsed = int.TryParse(category, out int categoryId);
                if (!isParsed)
                {
                    return BadRequest(new ErrorMessage("You have to give us number in your list of categories", StatusCodes.Status400BadRequest));
                }
                
                var categoryDb = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Categories, c => c.CategoryId == categoryId);
                if (categoryDb == null)
                {
                    return BadRequest(new ErrorMessage("We cannot found the category with this id: " + categoryId, StatusCodes.Status400BadRequest));
                }
                
                product.Categories.Add(categoryDb);
            }
        }
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return Ok(new ProductDto(product, true));
    }
    
    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="productId">The identifier of the product to update.</param>
    /// <param name="productName">The updated name of the product.</param>
    /// <param name="productBrandName">The updated name of the product's brand.</param>
    /// <param name="productDescription">The updated description of the product.</param>
    /// <param name="productPrice">The updated price of the product.</param>
    /// <param name="stockId">The updated identifier of the stock.</param>
    /// <param name="categories">The updated categories of the product.</param>
    /// <returns>Returns a ProductDto object that represents the updated product.</returns>
    [HttpPut("update/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> ProductUpdate(
        string productId,
        [FromForm] string productName, 
        [FromForm] string productBrandName, 
        [FromForm] string productDescription,
        [FromForm] int productPrice,
        [FromForm] int stockId,
        [FromForm] string categories
        )
    {
        if (string.IsNullOrEmpty(productId))
        {
            return BadRequest(new ErrorMessage("You need to provide an id for the product you want to update", StatusCodes.Status400BadRequest));
        }
        
        if (productName.Length <= ConstantRules.MaxLengthName)
        {
            return BadRequest(new ErrorMessage("Your product name is too long, max " + ConstantRules.MaxLengthName + " character", StatusCodes.Status400BadRequest));
        }
        
        if (productDescription.Length <= ConstantRules.MaxLengthDescription)
        {
            return BadRequest(new ErrorMessage("Your description is too long, max " + ConstantRules.MaxLengthDescription + " charaacter", StatusCodes.Status400BadRequest));
        }
        
        if (productPrice <= 0)
        {
            return BadRequest(new ErrorMessage("Price is required to create an product", StatusCodes.Status400BadRequest));
        }
        
        if (stockId <= 0)
        {
            return BadRequest(new ErrorMessage("You need to specify a stock", StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        if (!RoleChecker.HasSuperiorRole(userContext.Role, RoleEnum.ProductManager))
        {
            return Unauthorized(new ErrorMessage("You cannot create an product", StatusCodes.Status401Unauthorized));
        }

        bool isProductIdParsed = int.TryParse(productId, out int productIdParsed);
        if (!isProductIdParsed)
        {
            return BadRequest(new ErrorMessage("Your product id is not an number", StatusCodes.Status400BadRequest));
        }
        
        var productUpdatedWanted = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Products, p => p.ProductId == productIdParsed);
        if (productUpdatedWanted == null)
        {
            return BadRequest(new ErrorMessage("We cannot found the product with this id: " + productIdParsed, StatusCodes.Status400BadRequest));
        }
        
        if (!string.IsNullOrEmpty(categories))
        {
            productUpdatedWanted.Categories = new List<Category>();
            var categoriesList = categories.Split(",");
            
            foreach (var category in categoriesList)
            {
                bool isParsed = int.TryParse(category, out int categoryId);
                if (!isParsed)
                {
                    return BadRequest(new ErrorMessage("You have to give us number in your list of categories", StatusCodes.Status400BadRequest));
                }
                
                var categoryDb = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Categories, c => c.CategoryId == categoryId);
                if (categoryDb == null)
                {
                    return BadRequest(new ErrorMessage("We cannot found the category with this id: " + categoryId, StatusCodes.Status400BadRequest));
                }
                
                productUpdatedWanted.Categories.Add(categoryDb);
            }
        }

        _context.Products.Update(productUpdatedWanted);
        await _context.SaveChangesAsync();

        return Ok(new ProductDto(productUpdatedWanted, true));
    }

    /// <summary>
    /// Deletes an existing product.
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete("delete/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> ProductDelete(string productId)
    {
        if (string.IsNullOrEmpty(productId))
        {
            return BadRequest(new ErrorMessage("You need to provide an id for the product you want to delete", StatusCodes.Status400BadRequest));
        }
        
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        if (!RoleChecker.HasSuperiorRole(userContext.Role, RoleEnum.ProductManager))
        {
            return Unauthorized(new ErrorMessage("You cannot delete a product", StatusCodes.Status401Unauthorized));
        }
        
        bool isProductIdParsed = int.TryParse(productId, out int productIdParsed);
        if (!isProductIdParsed)
        {
            return BadRequest(new ErrorMessage("Your product id is not an number", StatusCodes.Status400BadRequest));
        }

        var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productIdParsed);
        if (productToDelete == null)
        {
            return BadRequest(new ErrorMessage("We cannot found the product with this id: " + productIdParsed, StatusCodes.Status400BadRequest));
        }
        
        _context.Products.Remove(productToDelete);
        await _context.SaveChangesAsync();
        
        return Ok(new ErrorMessage("Product deleted", StatusCodes.Status200OK));
    }
}