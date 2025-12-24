using backend.Dtos;
using backend.Dtos.Team;
using backend.Models;
using backend.Services.TeamValidation;

namespace backend.Services.TeamsValidation
{
    public interface ITeamValidationService
    {
        Task<ServiceResponse<TeamValidationData>> ValidateTeamCreationAsync(CreateTeamDto teamDto);
        Task<ServiceResponse<TeamValidationData>> ValidateTeamUpdateAsync(PutTeamDto teamDto);
    }
}
