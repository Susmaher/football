using Microsoft.AspNetCore.Routing.Constraints;

namespace backend.Dtos.MatchEvent
{
    public class GetMatchEventDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public int TeamPlayerId { get; set; }
        public string TeamPlayerName { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public int Minute { get; set; }
        public int ExtraMinute { get; set; }
    }
}
