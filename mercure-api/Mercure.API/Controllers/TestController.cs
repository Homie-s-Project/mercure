using Mercure.API.Context;
using Mercure.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

[TypeFilter(typeof(DevOnlyActionFilter))]
public class TestController : BaseController
{
    private readonly MercureContext _context;

    public TestController(MercureContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test-connected")]
    [AllowAnonymous]
    public IActionResult UserConnected()
    {
        var userContext = (User) HttpContext.Items["User"];

        if (userContext != null)
        {
            return Ok(new ErrorMessage("User connected", StatusCodes.Status200OK));
        }
        
        return Unauthorized(new ErrorMessage("No user connected", StatusCodes.Status401Unauthorized));
    }
}