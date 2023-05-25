using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Mercure.API.Utils.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mercure.API.Controllers;

/// <summary>
/// User controller allowing administrators to retrieve the list of users
/// </summary>
[Route("user")]
public class UserController : ApiSecurityController
{
    private readonly MercureContext _context;

    public UserController(MercureContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all users to display them in the admin panel.
    /// </summary>
    /// <returns></returns>
    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetAllUsers()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("You are not authenticated", StatusCodes.Status401Unauthorized));
        }

        var userRole = userContext.Role;
        if (!RoleChecker.HasRole(userRole, RoleEnum.Admin))
        {
            return Unauthorized(new ErrorMessage("You don't have the right to access this page.",
                StatusCodes.Status401Unauthorized));
        }

        // write log before the request because it can take a while and we want to keep track of it
        Logger.LogInfo("User (" + userContext.UserId + "/" + userContext.Role.RoleName +
                       ") is trying to get all users, this request can take a while.");

        // prepare the request to get all users with their role
        // if we don't do this request before the main one, the role will be null
        var roles = _context.Roles.ToList();
        Logger.LogInfo(
            "The request to get all roles has been successfully completed. (" + roles.Count + " roles found)");

        var users = _context.Users
            .Include(u => u.Role)
            .Select(u => new UserDto(u, true))
            .ToList();
        
        var userRoleAreNull = users.Any(u => u.Role == null);
        if (userRoleAreNull)
        {
            Logger.LogError(LogTarget.Database, "Error while getting all users, some users have no role.");
        }

        if (!users.Any())
        {
            return NotFound(new ErrorMessage("No users found.", StatusCodes.Status404NotFound));
        }

        // write log to keep track of the request
        Logger.LogInfo(
            "The request to get all users has been successfully completed. (" + users.Count + " users found)");

        return Ok(users);
    }
}