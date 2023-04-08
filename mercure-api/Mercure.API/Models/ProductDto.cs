using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercure.API.Models;

public class ProductDto
{
    public ProductDto(Product product)
    {
        ProductId = product.ProductId;
        ProductBrandName = product.ProductBrandName;
        ProductName = product.ProductName;
        ProductDescription = product.ProductDescription;
        ProductPrice = product.ProductPrice;
        ProductCreationDate = product.ProductCreationDate;
        ProductLastUpdate = product.ProductLastUpdate;
        
        if (product.Stock != null)
        {
            Stock = new StockDto(product.Stock);
        }
    }
    
    public ProductDto(Product product, bool loadMore)
    {
        ProductId = product.ProductId;
        ProductBrandName = product.ProductBrandName;
        ProductName = product.ProductName;
        ProductDescription = product.ProductDescription;
        ProductPrice = product.ProductPrice;
        ProductCreationDate = product.ProductCreationDate;
        ProductLastUpdate = product.ProductLastUpdate;
        
        if (product.Stock != null)
        {
            Stock = new StockDto(product.Stock);
        }

        if (loadMore)
        {
            if (product.Categories != null)
            {
                Categories = product.Categories.Select(c => new CategoryDto(c)).ToList();
            }                
        }
    }
    
    public int ProductId { get; set; }
    public string ProductBrandName { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int ProductPrice { get; set; }
    public DateTime ProductCreationDate { get; set; }
    public DateTime ProductLastUpdate { get; set; }
    
    public StockDto Stock { get; set; }
    public List<CategoryDto> Categories { get; set; }
}