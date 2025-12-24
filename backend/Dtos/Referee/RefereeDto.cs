using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Referee
{
    public class RefereeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
    }
}
