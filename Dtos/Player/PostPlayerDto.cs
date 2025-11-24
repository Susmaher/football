namespace backend.Dtos.Player
{
    public class PostPlayerDto
    {
        public string Name { get; set; }
        public DateOnly Birth_date { get; set; }
        public string? Position { get; set; }
    }
}
