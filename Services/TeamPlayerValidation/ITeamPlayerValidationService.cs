using backend.Dtos;
using backend.Dtos.TeamPlayer;

namespace backend.Services.TeamPlayerValidation
{
    public interface ITeamPlayerValidationService
    {
        Task<ServiceResponse<TeamPlayerValidationData>> ValidateTeamPlayerCreationAsync(PostTeamPlayerDto teamPlayerDto);
        Task<ServiceResponse<TeamPlayerValidationData>> ValidateTeamPlayerUpdateAsync(PutTeamPlayerDto teamPlayerDto);
    }
}
