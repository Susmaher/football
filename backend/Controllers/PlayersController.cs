using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.Player;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;
        public PlayersController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
        {
            var players = await _context.Players
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Birth_date = p.Birth_date,
                    Position = p.Position
                }).ToListAsync();

            return Ok(players);
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Where(p => p.Id == id)
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Birth_date = p.Birth_date,
                    Position = p.Position
                })
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound("Player not found");
            }

            return Ok(player);
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDto pl)
        {
            if (id != pl.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }
            
            if(await _validationService.NameAndBirthDateExistsAsync<Player>(pl.Name, pl.Birth_date, pl.Id))
            {
                return BadRequest("A player with this name and birth date already exists");
            }

            var player = new Player
            {
                Id = id,
                Name = pl.Name,
                Birth_date = pl.Birth_date,
                Position = pl.Position
            };

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _validationService.FindByIdAsync<Player>(id) == null)
                {
                    return NotFound("Player not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> PostPlayer(PostPlayerDto pl)
        {
            if(await _validationService.NameAndBirthDateExistsAsync<Player>(pl.Name, pl.Birth_date))
            { 
                return BadRequest("A player with this name and birth date already exists");
            }

            var player = new Player
            {
                Name = pl.Name,
                Birth_date = pl.Birth_date,
                Position = pl.Position
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var returnPlayer = new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                Birth_date = player.Birth_date,
                Position = player.Position
            };

            return CreatedAtAction("GetPlayer", new { id = player.Id }, returnPlayer);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            //if a player is deleted, then also the teamplayer is deleted
            var teamplayers = await _context.TeamPlayers
                .Where(tp => tp.PlayerId == id)
                .ToListAsync();
            if (teamplayers.Any())
            {
                _context.TeamPlayers.RemoveRange(teamplayers);
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
