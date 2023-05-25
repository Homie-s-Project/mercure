namespace Mercure.API.Models;

/// <summary>
/// User role dto
/// </summary>
public class RoleDto
{
    public RoleDto(string roleName, int roleNumber)
    {
        RoleName = roleName;
        RoleNumber = roleNumber;
    }
    
    public RoleDto(Role role)
    {
        RoleName = role.RoleName;
        RoleNumber = role.RoleNumber;
    }

    /// <summary>
    /// Role ID (null if not saved or different from the dto)
    /// </summary>
    public string? RoleID { get; set; }
    public string RoleName { get; set; }
    public int RoleNumber { get; set; }
}