using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Mercure.API.Models;

/// <summary>
/// User model
/// </summary>
public class User
{
    public User(string serviceId, string lastName, string firstName, DateTime? birthDate, string email,
        DateTime lastUpdatedAt)
    {
        ServiceId = serviceId;
        LastName = lastName;
        FirstName = firstName;
        BirthDate = birthDate;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        LastUpdatedAt = lastUpdatedAt;
    }

    public User()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }

    [Required] public string ServiceId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public DateTime? BirthDate { get; set; }
    [Required] public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    
    [ForeignKey("Role")] public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    
    public ICollection<Order> Orders { get; set; }
    
}