using backend.Models;

namespace backend.Services.MatchEventValidation
{
    public class MatchEventValidationData
    {
        public Event Event { get; set; }
        public Team Team { get; set; }
        public Player Player { get; set; }
    }
}
