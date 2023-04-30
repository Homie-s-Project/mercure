using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

[Route("delivery")]
public class DeliveryController : ApiSecurityController
{

    private readonly MercureContext _context;

    public DeliveryController(MercureContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Get all available delivery
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableDelivery()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        // On vérifie que l'utilisateur soit logisticien
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.Logistician))
        {
            return Unauthorized(new ErrorMessage("User is not authorized, only people higher or equal as logistician can access", StatusCodes.Status401Unauthorized));
        }
        
        var oderToDelivery = _context.OrderProducts.Where(op => op.Shipped == false).ToList();
        if (!oderToDelivery.Any())
        {
            return NotFound(new ErrorMessage("No order to delivery", StatusCodes.Status404NotFound));
        }
        
        // On anonymise les infos de l'acheteur
        foreach (var orderProduct in oderToDelivery)
        {
            orderProduct.Order = null;
        }
        
        return Ok(oderToDelivery);
    }

    /// <summary>
    /// Get one order if it's available and not shipped
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(string orderId)
    {
        var isParsed = int.TryParse(orderId, out int orderIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("The order id don't seem to be a number.", StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        // On vérifie que l'utilisateur soit logisticien
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.Logistician))
        {
            return Unauthorized(new ErrorMessage("User is not authorized, only people higher or equal as logistician can access", StatusCodes.Status401Unauthorized));
        }
        
        var orderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == orderIdParsed);
        if (orderProduct == null)
        {
            return NotFound(new ErrorMessage("No order found with this id", StatusCodes.Status404NotFound));
        }
        
        // On anonymise les infos de l'acheteur
        orderProduct.Order = null;
        
        return Ok(orderProduct);
    }
    
    /// <summary>
    /// Set order as shipped
    /// </summary>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("shipped/{orderId}")]
    public async Task<IActionResult> ShippedOrder(string orderId)
    {
        var isParsed = int.TryParse(orderId, out int orderIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("The order id don't seem to be a number.", StatusCodes.Status400BadRequest));
        }
        
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        // On vérifie que l'utilisateur soit logisticien
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.Logistician))
        {
            return Unauthorized(new ErrorMessage("User is not authorized, only people higher or equal as logistician can access", StatusCodes.Status401Unauthorized));
        }
        
        var orderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == orderIdParsed);
        if (orderProduct == null)
        {
            return NotFound(new ErrorMessage("No order found with this id", StatusCodes.Status404NotFound));
        }
        
        orderProduct.Shipped = true;
        _context.OrderProducts.Update(orderProduct);
        await _context.SaveChangesAsync();
        
        return Ok(new ErrorMessage("Order shipped", StatusCodes.Status200OK));
    }
}