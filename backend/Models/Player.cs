using backend.Services;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Player : IEntityWithNameAndBirthDate
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public DateOnly Birth_date { get; set; }
        [Required]
        public int PositionId { get; set; }
        public Position? Position { get; set; }

        public TeamPlayer? TeamPlayer { get; set; }
    }
}
