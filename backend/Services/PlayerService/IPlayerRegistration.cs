using backend.Dtos;
using backend.Dtos.Player;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services.PlayerService
{
    public interface IPlayerRegistration
    {
        Task<ServiceResponse<GetPlayerDto>> RegisterPlayerWithTeamAsync(PlayerRegistrationDto player);
        Task<ServiceResponse<GetPlayerDto>> ModifyPlayerWithTeamAsync();
    }
}
