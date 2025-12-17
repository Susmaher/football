using backend.Context;
using backend.Dtos;
using backend.Dtos.Player;
using backend.Services.PlayerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerRegistrationController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly IPlayerRegistration _playerRegistration;
        public PlayerRegistrationController(FootballDbContext context, IPlayerRegistration playerRegistration)
        {
            _context = context;
            _playerRegistration = playerRegistration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<GetPlayerDto>> RegisterPlayer(PlayerRegistrationDto player) 
        {
            var obj = await _playerRegistration.RegisterPlayerWithTeamAsync(player);

            return Ok(obj);
        }
    }
}
