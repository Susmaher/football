namespace backend.Dtos.Player
{
    public class PostPlayerDto
    {
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public int PositionId { get; set; }
    }
}
