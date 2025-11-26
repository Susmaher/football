namespace backend.Dtos.Player
{
    public class PutPlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Birth_date { get; set; }
        public int PositionId { get; set; }
    }
}
