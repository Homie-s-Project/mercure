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
        ProductType = product.ProductType;
        ProductInfo = product.ProductInfo;
        ProductPrice = product.ProductPrice;
        ProductCreationDate = product.ProductCreationDate;
        ProductLastUpdate = product.ProductLastUpdate;

        if (product.Stock != null)
        {
            Stock = new StockDto(product.Stock);
        }
    }

    public ProductDto(int orderId, Product product, List<OrderProduct> orderProducts)
    {
        ProductId = product.ProductId;
        ProductBrandName = product.ProductBrandName;
        ProductName = product.ProductName;
        ProductDescription = product.ProductDescription;
        ProductType = product.ProductType;
        ProductInfo = product.ProductInfo;
        ProductPrice = product.ProductPrice;
        ProductCreationDate = product.ProductCreationDate;
        ProductLastUpdate = product.ProductLastUpdate;
        ProductQuantity =
            (int) (orderProducts.FirstOrDefault(op =>
                op.Order.OrderId == orderId && op.Product.ProductId == product.ProductId)!.Quantity ?? 0);

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
        ProductType = product.ProductType;
        ProductInfo = product.ProductInfo;
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

    public ProductDto()
    {
    }

    protected ProductDto(ProductDto product)
    {
        ProductId = product.ProductId;
        ProductBrandName = product.ProductBrandName;
        ProductName = product.ProductName;
        ProductDescription = product.ProductDescription;
        ProductType = product.ProductType;
        ProductInfo = product.ProductInfo;
        ProductPrice = product.ProductPrice;
        ProductCreationDate = product.ProductCreationDate;
        ProductLastUpdate = product.ProductLastUpdate;

        if (product.Stock != null)
        {
            Stock = new StockDto(product.Stock);
        }
    }

    public int ProductId { get; set; }
    public string ProductBrandName { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }

    /// <summary>
    /// ProductType is a string where you can save the brand of the product
    /// </summary>
    public string ProductType { get; set; }

    /// <summary>
    /// ProductInfo is a string where you can save weight, size, etc.
    /// </summary>
    public string ProductInfo { get; set; }

    public int ProductPrice { get; set; }

    /// <summary>
    /// ProductQuantity is the quantity of the product in the stock only shown when getting a product from a order
    /// </summary>
    public int ProductQuantity { get; set; }

    public DateTime ProductCreationDate { get; set; }
    public DateTime ProductLastUpdate { get; set; }

    public StockDto Stock { get; set; }
    public List<CategoryDto> Categories { get; set; }
}