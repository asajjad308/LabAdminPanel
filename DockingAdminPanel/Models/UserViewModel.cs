using System.ComponentModel.DataAnnotations;

namespace DockingAdminPanel.Models
{
    public class UserViewModel
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(6)]
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserAvatar { get; set; }
    }
}
