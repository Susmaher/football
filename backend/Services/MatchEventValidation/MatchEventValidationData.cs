using backend.Models;

namespace backend.Services.MatchEventValidation
{
    public class MatchEventValidationData
    {
        public EventType EventType { get; set; }
        public Team Team { get; set; } = new Team();
        public Player Player { get; set; } = new Player();
    }
}
