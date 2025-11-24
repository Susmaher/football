using backend.Services;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Referee : IEntityWithNameAndBirthDate
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public DateOnly Birth_date { get; set; }
        public ICollection<Match>? Matches { get; set; }
    }
}
