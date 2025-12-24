namespace backend.Dtos.Team
{
    public class PutTeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Points { get; set; }
        public int DivisionId { get; set; }
        public int FieldId { get; set; }
    }
}
