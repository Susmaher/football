using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.Dtos.Match
{
    public class GetMatchDto
    {
        public int Id { get; set; }
        public DateTime? MatchDate { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public int Round { get; set; }
        public string Status { get; set; } = string.Empty;
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; } = string.Empty;
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; } = string.Empty;
        public int DivisionId { get; set; }
        public string DivisionName { get; set; } = string.Empty;
        public int RefereeId { get; set; }
        public string RefereeName { get; set; } = string.Empty;
        public int FieldId { get; set; }
        public string FieldName { get; set; } = string.Empty;
    }
}
