using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class OrderProduct
{
    public OrderProduct(int productId, int quantity, bool shipped)
    {
        ProductId = productId;
        Quantity = quantity;
        Shipped = shipped;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderProductId { get; set; }
    
    [ForeignKey("Product")] public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Quantity { get; set; }
    // True = livré, False = non livré
    public Boolean Shipped { get; set; }
}