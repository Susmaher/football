using System.ComponentModel.DataAnnotations;
using backend.Services;

namespace backend.Models
{
    public class Position : IEntityWithName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }
        public ICollection<Player>? Players { get; set; }
    }
}
