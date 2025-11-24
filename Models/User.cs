using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Username { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string PasswordHash { get; set; }
        public string? Role { get; set; }
    }
}
