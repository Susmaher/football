using backend.Context;
using backend.Dtos;
using backend.Dtos.Match;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.MatchValidation
{
    public class MatchValidationService : IMatchValidationService
    {
        private readonly ICommonValidationService _commonValidation;
        private readonly FootballDbContext _context;

        public MatchValidationService(FootballDbContext context, ICommonValidationService commonValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
        }
        public async Task<ServiceResponse<MatchValidationData>> ValidateMatchCreationAsync(PostMatchDto matchDto)
        {
            if (await MatchAlreadyExistAsync(matchDto.HomeTeamId, matchDto.AwayTeamId, matchDto.Round))
            {
                return new ServiceResponse<MatchValidationData> { Success = false, Message = "Match with this datas already exists" };
            }

            return await ValidateCommonMatchRulesAsync(matchDto.Status, matchDto.DivisionId, matchDto.AwayTeamId, matchDto.HomeTeamId, matchDto.Round, matchDto.FieldId, matchDto.RefereeId, matchDto.HomeScore, matchDto.AwayScore, matchDto.MatchDate);
        }

        public async Task<ServiceResponse<MatchValidationData>> ValidateMatchUpdateAsync(PutMatchDto matchDto)
        {
            if (await MatchAlreadyExistAsync(matchDto.HomeTeamId, matchDto.AwayTeamId, matchDto.Round, matchDto.Id))
            {
                return new ServiceResponse<MatchValidationData> { Success = false, Message = "Match with this datas already exists" };
            }

            return await ValidateCommonMatchRulesAsync(matchDto.Status, matchDto.DivisionId, matchDto.AwayTeamId, matchDto.HomeTeamId, matchDto.Round, matchDto.FieldId, matchDto.RefereeId, matchDto.HomeScore, matchDto.AwayScore, matchDto.MatchDate);
        }

        private async Task<ServiceResponse<MatchValidationData>> ValidateCommonMatchRulesAsync(string status, int divisionId, int AwayTeamId, int HomeTeamId, int round, int fieldId, int refereeId, int? homeScore = null, int? awayScore = null, DateTime? date = null)
        {
            var response = new ServiceResponse<MatchValidationData>();

            //check the status is valid
            if (!Enum.TryParse<MatchStatus>(status, true, out var stat))
            {
                response.Success = false;
                response.Message = "Invalid match status provided.";
                return response;
            }

            //check if teams exists
            var homeTeam = await _commonValidation.FindByIdAsync<Team>(HomeTeamId);
            var awayTeam = await _commonValidation.FindByIdAsync<Team>(AwayTeamId);
            if (homeTeam == null || awayTeam == null)
            {
                response.Success = false;
                response.Message = "One or both teams do not exist";
                return response;
            }

            //check if division exists
            var division = await _commonValidation.FindByIdAsync<Division>(divisionId);
            if (division == null)
            {
                response.Success = false;
                response.Message = "Division does not exist";
                return response;
            }


            //check if field exists
            var field = await _commonValidation.FindByIdAsync<Field>(fieldId);
            if (field == null)
            {
                response.Success = false;
                response.Message = "Field does not exist";
                return response;
            }

            //check if referee exists
            var referee = await _commonValidation.FindByIdAsync<Referee>(refereeId);
            if (referee == null)
            {
                response.Success = false;
                response.Message = "Referee does not exist";
                return response;
            }

            //check if datum is valid (played match cannot be in the future)
            if(date.HasValue && stat == MatchStatus.Played && date > DateTime.UtcNow)
            {
                response.Success = false;
                response.Message = "A played match cannot be in the future";
                return response;
            }

            //check if round is valid
            if(round < 0)
            {
                response.Success = false;
                response.Message = "Round cannot be a negative number";
                return response;
            }

            //if the match is not completed, scores must have null value
            if (stat != MatchStatus.Played && (homeScore != null || awayScore != null))
            {
                response.Success = false;
                response.Message = "You can’t assign scores to matches that haven’t been played.";
                return response;
            }

            //if the match is completed, it mush have homescore, awayscore, field, and referee
            if (stat == MatchStatus.Played && (homeScore == null || awayScore == null))
            {
                response.Success = false;
                response.Message = "Completed matches need scores";
                return response;
            }

            //check if scores are higher than zero
            if(stat == MatchStatus.Played && (homeScore < 0 || awayScore < 0))
            {
                response.Success = false;
                response.Message = "Score values cannot be negative";
                return response;
            }

            //the teams needs to have the same division as the match
            if (homeTeam.DivisionId != divisionId || awayTeam.DivisionId != divisionId)
            {
                response.Success = false;
                response.Message = "One or both teams do not belong to the specified division.";
                return response;
            }

            //hometeam cannot be equal to awayteam
            if (HomeTeamId == AwayTeamId)
            {
                response.Success = false;
                response.Message = "Home and away team cannot be the same.";
                return response;
            }

            response.data = new MatchValidationData { Status = stat, HomeTeam = homeTeam, AwayTeam = awayTeam, Division = division, Field = field, Referee = referee }; 
            return response;
        }

        private async Task<bool> MatchAlreadyExistAsync(int home, int away, int round, int? id = null)
        {
            var sameValue = _context.Matches.Where(m => m.HomeTeamId == home && m.AwayTeamId == away && m.Round == round);

            if (id.HasValue)
            {
                sameValue = sameValue.Where(m => m.Id != id.Value);
            }

            return await sameValue.AnyAsync();
        }
    }
}
