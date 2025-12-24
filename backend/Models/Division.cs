using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Division : IEntityWithName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        public ICollection<Team>? Teams { get; set; }
        public ICollection<Match>? Matches { get; set; }
    }
}
