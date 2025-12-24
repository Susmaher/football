using backend.Context;
using backend.Dtos;
using backend.Dtos.Player;
using backend.Dtos.TeamPlayer;
using backend.Models;
using backend.Services.TeamPlayerValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services.PlayerService
{
    public class PlayerRegistration : IPlayerRegistration
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;
        private readonly ITeamPlayerValidationService _teamPlayerValidation;
        public PlayerRegistration(FootballDbContext context, ICommonValidationService validationService, ITeamPlayerValidationService teamPlayerValidation)
        {
            _context = context;
            _validationService = validationService;
            _teamPlayerValidation = teamPlayerValidation;
        }
        public async Task<ServiceResponse<GetPlayerDto>> RegisterPlayerWithTeamAsync(PlayerRegistrationDto player)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var validationResponse = await _validationService.NameAndBirthDateExistsAsync<Player>(player.Name, player.BirthDate);
                if (!validationResponse.Success)
                {
                    throw new Exception(validationResponse.Message);
                }

                var position = await _validationService.FindByIdAsync<Position>(player.PositionId);
                if (position == null)
                {
                    throw new Exception("Position not found");
                }

                var newPlayer = new Player { Name = player.Name, BirthDate = player.BirthDate, PositionId = player.PositionId };
                _context.Players.Add(newPlayer);
                await _context.SaveChangesAsync();

                var tp = await _teamPlayerValidation.ValidateTeamPlayerCreationAsync(new PostTeamPlayerDto { TeamId = player.TeamId, PlayerId = newPlayer.Id });
                if (!tp.Success)
                {
                    throw new Exception(tp.Message);
                }

                var newTeamPlayer = new TeamPlayer
                {
                    TeamId = player.TeamId,
                    PlayerId = newPlayer.Id,
                };

                _context.TeamPlayers.Add(newTeamPlayer);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new ServiceResponse<GetPlayerDto>
                {
                    Success = true,
                    data = new GetPlayerDto
                    {
                        Id = newPlayer.Id,
                        Name = player.Name,
                        BirthDate = player.BirthDate,
                        PositionId = player.PositionId,
                        PositionName = position.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResponse<GetPlayerDto>
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public Task<ServiceResponse<GetPlayerDto>> ModifyPlayerWithTeamAsync()
        {
            throw new NotImplementedException();
        }
    }
}
