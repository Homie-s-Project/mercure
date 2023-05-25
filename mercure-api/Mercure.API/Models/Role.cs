using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

/// <summary>
/// User role
/// </summary>
[Table("UserRole")]
public class Role
{
    public Role()
    {
    }

    public Role(string roleName, int roleNumber)
    {
        RoleName = roleName;
        RoleNumber = roleNumber;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }
    public string RoleName { get; set;}
    public int RoleNumber { get; set; }
}