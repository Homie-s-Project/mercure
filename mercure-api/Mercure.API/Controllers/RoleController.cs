using System.Linq;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mercure.API.Controllers;

[Route("/roles")]
public class RoleController : ApiSecurityController
{
    private readonly MercureContext _context;

    public RoleController(MercureContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetRoles()
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }

        var roles = _context.Roles
            .OrderByDescending(r => r.RoleNumber)
            .Select(r => new RoleDto(r))
            .ToList();

        if (!roles.Any())
        {
            return NotFound(new ErrorMessage("No roles found", StatusCodes.Status404NotFound));
        }

        return Ok(roles);
    }

    /// <summary>
    /// Get one role by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{roleId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> GetRole(string roleId)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }

        bool isParsed = int.TryParse(roleId, out int roleIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("You have to give us a valid role id", StatusCodes.Status400BadRequest));
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleIdParsed);
        if (role == null)
        {
            return NotFound(new ErrorMessage("No role found", StatusCodes.Status404NotFound));
        }

        return Ok(new RoleDto(role));
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="roleName">name</param>
    /// <param name="roleNumber">number of priority</param>
    /// <returns></returns>
    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> CreateRole(
        [FromForm] string roleName,
        [FromForm] string roleNumber)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            return BadRequest(new ErrorMessage("Role name is required", StatusCodes.Status400BadRequest));
        }

        if (string.IsNullOrEmpty(roleNumber))
        {
            return BadRequest(new ErrorMessage("Role number is required", StatusCodes.Status400BadRequest));
        }

        bool isParsed = int.TryParse(roleNumber, out int roleNumberParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("You have to give us a valid number", StatusCodes.Status400BadRequest));
        }

        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }

        if (userContext.Role.RoleNumber != (int) RoleEnum.Admin)
        {
            return Unauthorized(new ErrorMessage("You don't have the right to create a Role",
                StatusCodes.Status401Unauthorized));
        }

        var role = new Role
        {
            RoleName = roleName,
            RoleNumber = roleNumberParsed
        };

        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return Ok(new RoleDto(role));
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    [HttpDelete("delete/{roleId}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }

        bool isParsed = int.TryParse(roleId, out int roleIdParsed);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("You have to give us a valid role id", StatusCodes.Status400BadRequest));
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleIdParsed);
        if (role == null)
        {
            return NotFound(new ErrorMessage("No role found", StatusCodes.Status404NotFound));
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return Ok(new RoleDto(role));
    }

    /// <summary>
    /// Change the role of another user, only admin can do that
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleNumber"></param>
    /// <returns></returns>
    [HttpPost("update/{userId}/{roleNumber}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> UpdateRoleUser(string userId, string roleNumber)
    {
        var userContext = (User) HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }

        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.Admin))
        {
            return Unauthorized(new ErrorMessage("You don't have the right to update a Role",
                StatusCodes.Status401Unauthorized));
        }
        
        bool isRoleIdParsed = int.TryParse(roleNumber, out int roleIdParsed);
        if (!isRoleIdParsed)
        {
            return BadRequest(new ErrorMessage("You have to give us a valid role id", StatusCodes.Status400BadRequest));
        }
        
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleIdParsed);
        if (role == null)
        {
            return NotFound(new ErrorMessage("No role found", StatusCodes.Status404NotFound));
        }
        
        bool isUserIdParsed = int.TryParse(userId, out int userIdParsed);
        if (!isUserIdParsed)
        {
            return BadRequest(new ErrorMessage("You have to give us a valid user id", StatusCodes.Status400BadRequest));
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userIdParsed);
        if (user == null)
        {
            return NotFound(new ErrorMessage("No user found with the id : " + userIdParsed, StatusCodes.Status404NotFound));
        }
        
        user.Role = role;
        await _context.SaveChangesAsync();
        
        return Ok(new UserDto(user));
    }
}