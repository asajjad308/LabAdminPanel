using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DockingApi.Models
{
    public class Users : IdentityUser
    {
     
        public string FirstName { get; set; } = null!;

     
        public string LastName { get; set; } = null!;

         
        public string CompanyId { get; set; } = null!;
         
        public string Gender { get; set; } = null!;
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserAvatar { get; set; }

    }
}
