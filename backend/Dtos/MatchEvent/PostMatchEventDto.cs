namespace backend.Dtos.MatchEvent
{
    public class PostMatchEventDto
    {
        public int MatchId { get; set; }
        public int TeamId { get; set; }
        public int TeamPlayerId { get; set; }
        public int EventId { get; set; }
        public int Minute { get; set; }
        public int? ExtraMinute { get; set; }
    }
}
