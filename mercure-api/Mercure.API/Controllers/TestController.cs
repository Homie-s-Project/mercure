using System.Collections.Generic;
using System.Linq;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mercure.API.Controllers;

/// <summary>
/// Controller for test
/// </summary>
[TypeFilter(typeof(DevOnlyActionFilter))]
public class TestController : BaseController
{
    private readonly MercureContext _context;

    public TestController(MercureContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Route for test if user is connected
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

    /// <summary>
    /// Route for test if user database has dev user
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test-dev-users")]
    [AllowAnonymous]
    public IActionResult UserDevRoles()
    {
        var userRoles = _context.Users.Where(u => u.ServiceId.StartsWith("DEV"));
        if (!userRoles.Any())
        {
            BadRequest(new ErrorMessage("No dev user found in the database to give you.",
                StatusCodes.Status400BadRequest));
        }

        return Ok(new ErrorMessage("Dev user found in the database", StatusCodes.Status200OK));
    }

    /// <summary>
    /// Route that return all dev users to test the roles
    /// </summary>
    /// <returns></returns>
    [HttpGet("/test-roles")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserRole))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public IActionResult UserRoles()
    {
        var userRoles = _context.Users
            .Include(u => u.Role)
            .Where(u => u.ServiceId.StartsWith("DEV"));
        if (!userRoles.Any())
        {
            BadRequest(new ErrorMessage("No dev user found in the database to give you.",
                StatusCodes.Status400BadRequest));
        }

        var userRolesList = new List<UserRole>();
        foreach (var userRole in userRoles)
        {
            userRolesList.Add(new UserRole(userRole.Role.RoleName, JwtUtils.GenerateJsonWebToken(userRole)));
        }

        return Ok(userRolesList);
    }

    private class UserRole
    {
        public string RoleName { get; set; }
        public string Token { get; set; }

        public UserRole(string roleName, string token)
        {
            RoleName = roleName;
            Token = token;
        }
    }
}