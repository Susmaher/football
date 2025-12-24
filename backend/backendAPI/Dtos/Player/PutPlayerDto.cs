namespace backend.Dtos.Player
{
    public class PutPlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public int PositionId { get; set; }
    }
}
