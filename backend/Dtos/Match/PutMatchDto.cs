using backend.Models;

namespace backend.Dtos.Match
{
    public class PutMatchDto
    {
        public int Id { get; set; }
        public DateTime? Match_date { get; set; }
        public int? Home_score { get; set; }
        public int? Away_score { get; set; }
        public int Round { get; set; }
        public string Status { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int DivisionId { get; set; }
        public int? RefereeId { get; set; }
        public int? FieldId { get; set; }
    }
}
