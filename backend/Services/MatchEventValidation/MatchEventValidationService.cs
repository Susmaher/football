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
            if(await MatchEventAlreadyExists(matchEventDto.MatchId, matchEventDto.TeamId, matchEventDto.TeamPlayerId, matchEventDto.EventId, matchEventDto.Minute, matchEventDto.ExtraMinute))
            {
                return new ServiceResponse<MatchEventValidationData> { Success = false, Message = "Match Event with these datas already exists" };
            }

            return await ValidateCommonMatchEventRulesAsync(matchEventDto.TeamId, matchEventDto.MatchId, matchEventDto.TeamPlayerId, matchEventDto.EventId);
        }

        public async Task<ServiceResponse<MatchEventValidationData>> ValidateMatchEventUpdateAsync(PutMatchEventDto matchEventDto)
        {
            if (await MatchEventAlreadyExists(matchEventDto.MatchId, matchEventDto.TeamId, matchEventDto.TeamPlayerId, matchEventDto.EventId, matchEventDto.Minute, matchEventDto.ExtraMinute, matchEventDto.Id))
            {
                return new ServiceResponse<MatchEventValidationData> { Success = false, Message = "Match Event with these datas already exists" };
            }

            return await ValidateCommonMatchEventRulesAsync(matchEventDto.TeamId, matchEventDto.MatchId, matchEventDto.TeamPlayerId, matchEventDto.EventId);
        }

        private async Task<ServiceResponse<MatchEventValidationData>> ValidateCommonMatchEventRulesAsync(int teamId, int matchId, int teamPlayerId, int eventId) 
        { 
            var response = new ServiceResponse<MatchEventValidationData>();

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

            //check if event exists
            var @event = await _commonValidation.FindByIdAsync<Event>(eventId);
            if (@event == null)
            {
                response.Success = false;
                response.Message = "Event does not exist";
                return response;
            }
           
            //check if a player doesn't have more than 2 yellowcard
            if(@event.Name.Equals("yellow card", StringComparison.OrdinalIgnoreCase))
            {
                if (await _context.MatchEvents.Include(me => me.Event).Where(me => me.MatchId == matchId && me.TeamId == teamId && me.TeamPlayerId == teamPlayerId && me.EventId == eventId).CountAsync() >= 2)
                {
                    response.Success = false;
                    response.Message = "A player cannot have more than 2 yellow cards";
                    return response;
                }
            }

            //check if teamplayer belong to the team
            if(teamPlayer.Id != team.Id)
            {
                response.Success=false;
                response.Message = "The player does not belong to the team";
                return response;
            }

            //check if team is played on that match
            if(team.Id != match.HomeTeamId || team.Id != match.AwayTeamId)
            {
                response.Success=false;
                response.Message = "Team does not played on that match";
                return response;
            }

            var player = await _commonValidation.FindByIdAsync<Player>(teamPlayer.PlayerId);
            response.data = new MatchEventValidationData { Event = @event, Team = team, Player = player };
            return response;
        }

        private async Task<bool> MatchEventAlreadyExists(int matchid, int teamid, int tpid, int eventid, int minute, int? extra_minute = null, int? id = null)
        {
            var sameValue = _context.MatchEvents.Where(me => me.MatchId == matchid && me.TeamId == teamid && me.TeamPlayerId == tpid && me.EventId == eventid && me.Minute == minute);

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
