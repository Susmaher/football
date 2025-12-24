using backend.Services;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Player : IEntityWithNameAndBirthDate
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public TeamPlayer? TeamPlayer { get; set; }
    }
}
