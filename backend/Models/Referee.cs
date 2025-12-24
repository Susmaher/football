using backend.Services;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Referee : IEntityWithNameAndBirthDate
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateOnly BirthDate { get; set; }
        public ICollection<Match>? Matches { get; set; }
    }
}
