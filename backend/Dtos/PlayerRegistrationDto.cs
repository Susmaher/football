namespace backend.Dtos
{
    public class PlayerRegistrationDto
    {
        public string Name { get; set; }
        public DateOnly Birth_date { get; set; }
        public int PositionId { get; set; }
        public int TeamId { get; set; }
    }
}
