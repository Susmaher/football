using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Referee
{
    public class RefereeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Birth_date { get; set; }
    }
}
