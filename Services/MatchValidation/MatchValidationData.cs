using backend.Models;

namespace backend.Services.MatchValidation
{
    public class MatchValidationData
    {
        public MatchStatus Status { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Division Division { get; set; }
        public Referee Referee { get; set; }
        public Field Field { get; set; }
    }
}
