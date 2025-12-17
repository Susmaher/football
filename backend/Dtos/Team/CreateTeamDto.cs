namespace backend.Dtos.Team
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public int DivisionId { get; set; }

        public int FieldId { get; set; }

    }
}
