using System.Collections.Generic;

namespace Mercure.API.Models;

/// <summary>
/// Cart of the user
/// </summary>
public class Cart
{
    public Cart(string userId)
    {
        UserId = userId;
        Products = new List<CartProduct>();
    }

    /// <summary>
    /// Add a product to the cart
    /// </summary>
    /// <param name="product"></param>
    /// <param name="quantity"></param>
    public void AddProduct(Product product, int quantity=1)
    {
        var cartProduct = Products.Find(p => p.Product.ProductId == product.ProductId);
        if (cartProduct != null)
        {
            cartProduct.Quantity += quantity;
        }
        else
        {
            Products.Add(new CartProduct(product, quantity));
        }

    }

    /// <summary>
    /// Remove a product from the cart
    /// </summary>
    /// <param name="product"></param>
    public void RemoveProduct(Product product)
    {
        var cartProduct = Products.Find(p => p.Product.ProductId == product.ProductId);
        if (cartProduct == null)
        {
            return;
        }
        else
        {
            if (cartProduct.Quantity > 1)
            {
                cartProduct.Quantity -= 1;
                return;
            }
        }

        Products.Remove(cartProduct);
    }

    public string UserId { get; set; }
    public List<CartProduct> Products { get; set; }
}

public class CartProduct
{
    public CartProduct(Product product, int quantity)
    {
        Product = new ProductDto(product);
        Quantity = quantity;
    }

    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
}