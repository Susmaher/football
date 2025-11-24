using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Player
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Birth_date { get; set; }
        public string? Position { get; set; }
    }
}
