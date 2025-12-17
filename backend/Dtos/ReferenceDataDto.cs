using backend.Dtos.Division;
using backend.Dtos.Field;
using backend.Dtos.Player;
using backend.Dtos.Position;
using backend.Dtos.Referee;
using backend.Dtos.Team;
using backend.Models;

namespace backend.Dtos
{
    public class ReferenceDataDto
    {
        public GetTeamsDto? Team { get; set; }
        public DivisionDto? Division { get; set; }
        public FieldDto? Field { get; set; }
        public GetPlayerDto? Player { get; set; }
        public PositionDto? Position { get; set; }
        public RefereeDto? Referee { get; set; }
    }
}
