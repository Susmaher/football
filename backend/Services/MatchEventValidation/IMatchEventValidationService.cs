using backend.Dtos;
using backend.Dtos.MatchEvent;
using backend.Dtos.Team;
using backend.Services.TeamValidation;

namespace backend.Services.MatchEventValidation
{
    public interface IMatchEventValidationService
    {
        Task<ServiceResponse<MatchEventValidationData>> ValidateMatchEventCreationAsync(PostMatchEventDto teamDto);
        Task<ServiceResponse<MatchEventValidationData>> ValidateMatchEventUpdateAsync(PutMatchEventDto teamDto);
    }
}
