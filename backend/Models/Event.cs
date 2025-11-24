using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Event : IEntityWithName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        public ICollection<MatchEvent>? MatchEvents { get; set; }
    }
}
