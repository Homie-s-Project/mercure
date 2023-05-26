using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Order
{
    public Order(int orderPrice, int orderTaxPrice, int orderDeliveryPrice, DateTime orderDate, bool orderStatus)
    {
        OrderPrice = orderPrice;
        OrderTaxPrice = orderTaxPrice;
        OrderDeliveryPrice = orderDeliveryPrice;
        OrderDate = orderDate;
        OrderStatus = orderStatus;
    }

    public Order()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
    public long? OrderPrice { get; set; }
    public int OrderTaxPrice { get; set; }
    public int OrderDeliveryPrice { get; set; }
    public string? SessionId { get; set; }
    public DateTime OrderDate { get; set; }
    // True = payé, False = non payé
    /// <summary>
    /// True = payé, False = non payé
    /// </summary>
    public Boolean OrderStatus { get; set; }
    
    [ForeignKey("User")] public int? UserId { get; set; }
    public virtual User User { get; set; }
    
    public  ICollection<OrderProduct> Products { get; set; }
    public  ICollection<Animal> Animals { get; set; }
}