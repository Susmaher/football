using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(TeamId), nameof(PlayerId), IsUnique = true)]
    public class TeamPlayer : IEntityWithId
    {
        public int Id { get; set; }
        [Required]
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        public ICollection<MatchEvent>? MatchEvents { get; set; }
    }
}
