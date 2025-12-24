using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.Dtos.Match
{
    public class PostMatchDto
    {
        public DateTime? MatchDate { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public int Round { get; set; }
        public string Status { get; set; } = string.Empty;
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int DivisionId { get; set; }
        public int RefereeId { get; set; }
        public int FieldId { get; set; }
    }
}
