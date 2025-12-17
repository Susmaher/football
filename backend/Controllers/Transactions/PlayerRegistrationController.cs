using backend.Context;
using backend.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerRegistrationController : ControllerBase
    {
        private readonly FootballDbContext _context;

        public PlayerRegistrationController(FootballDbContext context)
        {
            _context = context;
        }

        public async Task<int> RegisterPlayer(PlayerRegistrationDto player) 
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var newPlayer = 
            }
            catch
            {
                //alma
            }
        }
    }
}
