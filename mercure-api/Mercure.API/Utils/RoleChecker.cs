using Mercure.API.Models;

namespace Mercure.API.Utils;

/// <summary>
/// Role checker
/// </summary>
public static class RoleChecker
{
    /// <summary>
    /// Check if the user has the role
    /// </summary>
    /// <param name="userRole">role of user</param>
    /// <param name="roleEnum">role to have</param>
    /// <returns></returns>
    public static bool HasRole(Role userRole, RoleEnum roleEnum)
    {
        return userRole.RoleNumber >= (int) roleEnum;
    }
}