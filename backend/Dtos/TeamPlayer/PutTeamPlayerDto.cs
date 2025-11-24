namespace backend.Dtos.TeamPlayer
{
    public class PutTeamPlayerDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
    }
}
