using backend.Models;

namespace backend.Services.TeamValidation
{
    public class TeamValidationData
    {
        public Division Division { get; set; } = new Division();
        public Field Field { get; set; } = new Field();
    }
}
