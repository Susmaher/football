namespace backend.Dtos.TeamPlayer
{
    public class GetTeamPlayerDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}
