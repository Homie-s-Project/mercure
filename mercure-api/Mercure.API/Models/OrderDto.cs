using System;
using System.Collections.Generic;
using System.Linq;
using Mercure.API.Utils.Logger;

namespace Mercure.API.Models;

public class OrderDto
{
    public OrderDto(Order order)
    {
        OrderNumber = order.OrderId;
        OrderStatus = order.OrderStatus;
        OrderPrice = order.OrderPrice;
        OrderDate = order.OrderDate;
        if (order.Products != null)
        {
            Products = order.Products.Select(p => new ProductDto(p.Product)).ToList();
        }
    }

    public OrderDto(List<OrderProduct> orders, Order order)
    {
        if (!orders.Any())
        {
            Logger.LogError("OrderDto constructor: orders is empty");
        }
        
        var firstOrder = orders.FirstOrDefault(op => op.OrderId == order.OrderId)?.Order;
        if (firstOrder != null)
        {
            OrderNumber = firstOrder.OrderId;
            OrderStatus = firstOrder.OrderStatus;
            OrderPrice = firstOrder.OrderPrice;
            OrderDate = firstOrder.OrderDate;
        }

        Products = orders.Where(op => op.OrderId == order.OrderId).Select(o => new ProductDto(order.OrderId, o.Product, orders)).ToList();
    }

    public int OrderNumber { get; set; }
    public bool OrderStatus { get; set; }
    public long? OrderPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ProductDto> Products { get; set; }
}