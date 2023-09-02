using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DockingAdminPanel.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string? FirstName { get; set; } 

    [MaxLength(50)]
    public string? LastName { get; set; } 

 
    [MaxLength(6)]
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? RefreshToken { get; set; }
    public string? UserAvatar { get; set; }
}

