namespace backend.Dtos.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public int DivisionId { get; set; }
        public int FieldId { get; set; }

    }
}
