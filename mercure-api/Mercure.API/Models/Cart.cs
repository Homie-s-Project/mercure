using System.Collections.Generic;

namespace Mercure.API.Models;

public class Cart
{
    public Cart(int userId)
    {
        UserId = userId;
        Products = new List<CartProduct>();
    }

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

    public int UserId { get; set; }
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