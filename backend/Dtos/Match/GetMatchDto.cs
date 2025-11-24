using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.Dtos.Match
{
    public class GetMatchDto
    {
        public int Id { get; set; }
        public DateTime? Match_date { get; set; }
        public int? Home_score { get; set; }
        public int? Away_score { get; set; }
        public int Round { get; set; }
        public string Status { get; set; }
        public int HomeTeamId { get; set; }
        public string HomeTeamName { get; set; }
        public int AwayTeamId { get; set; }
        public string AwayTeamName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string? RefereeName { get; set; }
        public string? FieldName { get; set; }
    }
}
