using backend.Context;
using backend.Dtos;
using backend.Dtos.MatchEvent;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace backend.Services.MatchEventValidation
{
    public class MatchEventValidationService : IMatchEventValidationService
    {
        private readonly ICommonValidationService _commonValidation;
        private readonly FootballDbContext _context;
        public MatchEventValidationService(FootballDbContext context, ICommonValidationService commonValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
        }
        public async Task<ServiceResponse<MatchEventValidationData>> ValidateMatchEventCreationAsync(PostMatchEventDto matchEventDto)
        {
            if(await MatchEventAlreadyExists(matchEventDto.MatchId, matchEventDto.TeamId, matchEventDto.TeamPlayerId, matchEventDto.EventType, matchEventDto.Minute, matchEventDto.ExtraMinute))
            {
                return new ServiceResponse<MatchEventValidationData> { Success = false, Message = "Match Event with these datas already exists" };
            }

            return await ValidateCommonMatchEventRulesAsync(matchEventDto.TeamId, matchEventDto.MatchId, matchEventDto.TeamPlayerId, matchEventDto.EventType);
        }

        public async Task<ServiceResponse<MatchEventValidationData>> ValidateMatchEventUpdateAsync(PutMatchEventDto matchEventDto)
        {
            if (await MatchEventAlreadyExists(matchEventDto.MatchId, matchEventDto.TeamId, matchEventDto.TeamPlayerId, matchEventDto.EventType, matchEventDto.Minute, matchEventDto.ExtraMinute, matchEventDto.Id))
            {
                return new ServiceResponse<MatchEventValidationData> { Success = false, Message = "Match Event with these datas already exists" };
            }

            return await ValidateCommonMatchEventRulesAsync(matchEventDto.TeamId, matchEventDto.MatchId, matchEventDto.TeamPlayerId, matchEventDto.EventType);
        }

        private async Task<ServiceResponse<MatchEventValidationData>> ValidateCommonMatchEventRulesAsync(int teamId, int matchId, int teamPlayerId, string eventType) 
        { 
            var response = new ServiceResponse<MatchEventValidationData>();

            //check if the eventType valid
            if (!Enum.TryParse<EventType>(eventType, true, out var evType))
            {
                response.Success = false;
                response.Message = "Invalid event type provided.";
                return response;
            }

            //check if team exists
            var team = await _commonValidation.FindByIdAsync<Team>(teamId);
            if (team == null)
            {
                response.Success = false;
                response.Message = "Team does not exist";
                return response;
            }

            //check if match is exists
            var match = await _commonValidation.FindByIdAsync<Match>(matchId);
            if (match == null)
            {
                response.Success = false;
                response.Message = "Match does not exist";
                return response;
            }

            //check if teamplayer exists
            var teamPlayer = await _commonValidation.FindByIdAsync<TeamPlayer>(teamPlayerId);
            if (teamPlayer == null)
            {
                response.Success = false;
                response.Message = "Teamplayer does not exist";
                return response;
            }
           
            //check if a player doesn't have more than 2 yellowcard
            if(evType == EventType.YellowCard)
            {
                if (await _context.MatchEvents.Where(me => me.MatchId == matchId && me.TeamId == teamId && me.TeamPlayerId == teamPlayerId).CountAsync() > 2)
                {
                    response.Success = false;
                    response.Message = "A player cannot have more than 2 yellow cards";
                    return response;
                }
            }

            //check if teamplayer belong to the team
            if(teamPlayer.TeamId != team.Id)
            {
                response.Success=false;
                response.Message = "The player does not belong to the team";
                return response;
            }

            //check if team is played on that match
            if(team.Id != match.HomeTeamId && team.Id != match.AwayTeamId)
            {
                response.Success=false;
                response.Message = "Team does not played on that match";
                return response;
            }

            var player = await _commonValidation.FindByIdAsync<Player>(teamPlayer.PlayerId);
            response.data = new MatchEventValidationData { EventType = evType, Team = team, Player = player };
            return response;
        }

        private async Task<bool> MatchEventAlreadyExists(int matchid, int teamid, int tpid, string eventType, int minute, int? extra_minute = null, int? id = null)
        {
            var sameValue = _context.MatchEvents.Where(me => me.MatchId == matchid && me.TeamId == teamid && me.TeamPlayerId == tpid && me.EventType.ToString() == eventType && me.Minute == minute);

            if (extra_minute.HasValue)
            {
                sameValue = sameValue.Where(me => me.ExtraMinute == extra_minute);
            }

            if (id.HasValue)
            {
                sameValue = sameValue.Where(me => me.Id != id);
            }

            return await sameValue.AnyAsync();
        }
    }
}
