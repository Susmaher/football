using backend.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Index(nameof(DivisionId))]
    public class Team : IEntityWithName
    {
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
        public int Points { get; set; }
        [Required]
        public int DivisionId { get; set; }
        public Division? Division { get; set; }

        [Required]
        public int FieldId { get; set; }
        public Field? Field { get; set; }
        public ICollection<TeamPlayer>? Teamplayer { get; set; }
        public ICollection<Match>? HomeMatch { get; set; }
        public ICollection<Match>? AwayMatch { get; set; }
    }
}
