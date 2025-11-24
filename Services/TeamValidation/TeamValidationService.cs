using backend.Dtos;
using backend.Dtos.Team;
using backend.Models;
using backend.Services.TeamsValidation;

namespace backend.Services.TeamValidation
{
    public class TeamValidationService : ITeamValidationService
    {
        private readonly ICommonValidationService _commonValidation;

        public TeamValidationService(ICommonValidationService commonValidation) 
        { 
            _commonValidation = commonValidation;        
        }

        public async Task<ServiceResponse<TeamValidationData>> ValidateTeamCreationAsync(CreateTeamDto teamDto)
        {
            if (await _commonValidation.NameExistsAsync<Team>(teamDto.Name))
            {
                return new ServiceResponse<TeamValidationData> { Success = false, Message = "A team with this name already exists" };
            }

            return await ValidateCommonTeamRulesAsync(teamDto.DivisionId, teamDto.FieldId, teamDto.Points);
        }
        public async Task<ServiceResponse<TeamValidationData>> ValidateTeamUpdateAsync(PutTeamDto teamDto)
        {
            if (await _commonValidation.NameExistsAsync<Team>(teamDto.Name, teamDto.Id))
            {
                return new ServiceResponse<TeamValidationData> { Success = false, Message = "A team with this name already exists" };
            }

            return await ValidateCommonTeamRulesAsync(teamDto.DivisionId, teamDto.FieldId, teamDto.Points);
        }

        private async Task<ServiceResponse<TeamValidationData>> ValidateCommonTeamRulesAsync(int divisionId, int fieldId, int? points)
        {
            var response = new ServiceResponse<TeamValidationData>();

            if(points.HasValue && points.Value < 0)
            {
                response.Success = false;
                response.Message = "Points cannot be negative";
                return response;
            }

            var division = await _commonValidation.FindByIdAsync<Division>(divisionId);
            if (division == null)
            {
                response.Success = false;
                response.Message = "Division does not exist";
                return response;
            }

            var field = await _commonValidation.FindByIdAsync<Field>(fieldId);
            if (field == null)
            {
                response.Success = false;
                response.Message = "Field does not exist";
                return response;
            }

            response.data = new TeamValidationData { Division = division, Field = field };
            return response;
        }
    }
}
