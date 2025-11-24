using backend.Services;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Field : IEntityWithName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }
        public ICollection<Match>? Matches { get; set; }

        public ICollection<Team>? Teams { get; set; }
    }
}
