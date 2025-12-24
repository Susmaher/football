using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Player
{
    public class GetPlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
    }
}
