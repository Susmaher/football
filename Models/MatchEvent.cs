using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(MatchId))]
    public class MatchEvent : IEntityWithId
    {
        public int Id { get; set; }
        [Required]
        public int MatchId { get; set; }
        public Match? Match { get; set; }
        [Required]
        public int TeamId { get; set; }
        public Team? Team { get; set; }
        [Required]
        public int TeamPlayerId { get; set; }
        public TeamPlayer? TeamPlayer { get; set; }
        [Required]
        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        [Range(0, 120)]
        public int Minute { get; set; }
        [Range(0, 15)]
        public int? ExtraMinute { get; set; }
    }
}
