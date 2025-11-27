using backend.Context;
using backend.Dtos;
using backend.Dtos.TeamPlayer;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.TeamPlayerValidation
{
    public class TeamPlayerValidationService : ITeamPlayerValidationService
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _commonValidation;

        public TeamPlayerValidationService(FootballDbContext context, ICommonValidationService commonValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
        }

        public async Task<ServiceResponse<TeamPlayerValidationData>> ValidateTeamPlayerCreationAsync(PostTeamPlayerDto teamPlayerDto)
        {
            //checks if the player is already assigned to a team
            if (await TeamPlayerAlreadyExists(teamPlayerDto.TeamId, teamPlayerDto.PlayerId))
            {
                return new ServiceResponse<TeamPlayerValidationData> { Success = false, Message = "This player is already assigned to a team" };
            }

            return await ValidateCommonTeamPlayerRulesAsync(teamPlayerDto.TeamId, teamPlayerDto.PlayerId);
        }

        public async Task<ServiceResponse<TeamPlayerValidationData>> ValidateTeamPlayerUpdateAsync(PutTeamPlayerDto teamPlayerDto)
        {
            //checks if the player is already assigned to a team
            if (await TeamPlayerAlreadyExists(teamPlayerDto.TeamId, teamPlayerDto.PlayerId, teamPlayerDto.Id))
            {
                return new ServiceResponse<TeamPlayerValidationData> { Success = false, Message = "This player is already assigned to a team" };
            }

            return await ValidateCommonTeamPlayerRulesAsync(teamPlayerDto.TeamId, teamPlayerDto.PlayerId);
        }

        private async Task<ServiceResponse<TeamPlayerValidationData>> ValidateCommonTeamPlayerRulesAsync(int teamId, int playerId)
        {
            var response = new ServiceResponse<TeamPlayerValidationData>();

            //check if team exists
            var team = await _commonValidation.FindByIdAsync<Team>(teamId);
            if (team == null)
            {
                response.Success = false;
                response.Message = "Team not found";
                return response;
            }

            //check if player exists
            var player = await _commonValidation.FindByIdAsync<Player>(playerId);
            if (player == null)
            {
                response.Success = false;
                response.Message = "Player not found";
                return response;
            }

            //check if player not in another team yet

            response.data = new TeamPlayerValidationData { Team = team, Player = player };
            return response;
        }
        private async Task<bool> TeamPlayerAlreadyExists(int teamId, int playerId, int? id = null)
        {
            var sameValue = _context.TeamPlayers.Where(tp => tp.PlayerId == playerId);

            if (id.HasValue)
            {
                sameValue = sameValue.Where(tp => tp.Id != id);
            }

            return await sameValue.AnyAsync();
        } 
    }
}
