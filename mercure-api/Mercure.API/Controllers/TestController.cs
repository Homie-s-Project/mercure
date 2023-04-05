using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
[Route("dev")]
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
    [HttpGet("test-connected")]
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
    [HttpGet("test-dev-users")]
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
    [HttpGet("test-roles")]
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

    /// <summary>
    /// Get all roles for dev
    /// </summary>
    /// <returns></returns>
    [HttpGet("roles")]
    [AllowAnonymous]
    public IActionResult GetRoles()
    {
        var roles = _context.Roles
            .Select(r => new {r.RoleName, r.RoleNumber})
            .Distinct()
            .OrderByDescending(r => r.RoleNumber)
            .ToList();
        return Ok(roles);
    }
    
    /// <summary>
    /// Set roles for current user, has to be conneceted
    /// </summary>
    /// <returns></returns>
    [HttpPost("roles/{roleNumber}")]
    public async Task<IActionResult> SetRole(string roleNumber)
    {
        if (string.IsNullOrEmpty(roleNumber))
        {
            return BadRequest(new ErrorMessage("You need to provide a role number", StatusCodes.Status400BadRequest));
        }
        
        bool isRoleNumberParsed = int.TryParse(roleNumber, out int roleNumberParsed);
        if (!isRoleNumberParsed)
        {
            return BadRequest(new ErrorMessage("Your role number is not an number", StatusCodes.Status400BadRequest));
        }
        
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        
        if (userContext.Role.RoleNumber == roleNumberParsed)
        {
            return Ok(new ErrorMessage("Role already set", StatusCodes.Status200OK));
        }
        
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleNumber == roleNumberParsed);
        if (role == null)
        {
            return BadRequest(new ErrorMessage("Role not found", StatusCodes.Status400BadRequest));
        }
        
        userContext.Role = role;
        await _context.SaveChangesAsync();
        
        return Ok(new ErrorMessage("Role set", StatusCodes.Status200OK));
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