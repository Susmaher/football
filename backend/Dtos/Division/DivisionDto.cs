using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Division
{
    public class DivisionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
