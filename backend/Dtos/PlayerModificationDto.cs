namespace backend.Dtos
{
    public class PlayerModificationDto
    {
        public int PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public int PositionId { get; set; }
        public int TeamId { get; set; }
    }
}
