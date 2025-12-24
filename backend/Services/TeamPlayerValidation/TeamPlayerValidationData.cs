using backend.Models;

namespace backend.Services.TeamPlayerValidation
{
    public class TeamPlayerValidationData
    {
        public Player Player { get; set; } = new Player();
        public Team Team { get; set; } = new Team();
    }
}
