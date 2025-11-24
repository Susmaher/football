using backend.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(HomeTeamId))]
    [Index(nameof(AwayTeamId))]
    [Index(nameof(DivisionId))]
    public class Match : IEntityWithId
    {
        public int Id { get; set; }
        public DateTime? Match_date { get; set; }
        [Range(0, 30)]
        public int? Home_score { get; set; }
        [Range(0, 30)]
        public int? Away_score { get; set; }
        [Required]
        [Range(0, 30)]
        public int Round { get; set; }
        [Required]
        public MatchStatus Status { get; set; }
        [Required]
        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }
        [Required]
        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }

        public ICollection<MatchEvent>? Match_Events { get; set; }
        [Required]
        public int DivisionId { get; set; }
        public Division? Division { get; set; }

        public int? RefereeId { get; set; }
        public Referee? Referee { get; set; }
        public int? FieldId { get; set; }
        public Field? Field { get; set; }
    }
}
