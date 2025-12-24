using backend.Models;

namespace backend.Services.MatchValidation
{
    public class MatchValidationData
    {
        public MatchStatus Status { get; set; }
        public Team HomeTeam { get; set; } = new Team();
        public Team AwayTeam { get; set; } = new Team();
        public Division Division { get; set; } = new Division();
        public Referee Referee { get; set; } = new Referee();
        public Field Field { get; set; } = new Field();
    }
}
