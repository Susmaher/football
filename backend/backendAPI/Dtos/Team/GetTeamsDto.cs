using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.Team
{
    public class GetTeamsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Points { get; set; }
        public int DivisionId { get; set; }
        public string? DivisionName { get; set; }
        public int FieldId { get; set; }
        public string? FieldName { get; set; }
    }
}
