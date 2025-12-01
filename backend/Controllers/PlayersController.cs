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
using backend.Dtos.TeamPlayer;

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

        // GET: api/Players?position=1
        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPlayerDto>>> GetPlayers([FromQuery] int? position = null)
        {
            var query = _context.Players.AsQueryable();

            if (position.HasValue)
            {
                if(await _validationService.FindByIdAsync<Position>(position.Value) == null)
                {
                    return BadRequest("Position not found");
                }

                query = query.Where(p => p.PositionId == position);
            }

            var players = await query
                .Select(p => new GetPlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Birth_date = p.Birth_date,
                    PositionId = p.PositionId,
                    PositionName = p.Position!.Name
                }).ToListAsync();

            return Ok(players);
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPlayerDto>> GetPlayer(int id)
        {
            var player = await _context.Players
                .Where(p => p.Id == id)
                .Select(p => new GetPlayerDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Birth_date = p.Birth_date,
                    PositionId = p.PositionId,
                    PositionName = p.Position!.Name
                })
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return NotFound("Player not found");
            }

            return Ok(player);
        }

        //GET: api/Players/5/team
        [HttpGet("{id}/team")]
        public async Task<ActionResult<GetTeamPlayerDto>> GetPlayersTeam(int id)
        {
            var teamPlayer = await _context.TeamPlayers
                .Where(tp => tp.PlayerId == id)
                .Select(tp => new GetTeamPlayerDto
                {
                    Id = tp.PlayerId,
                    TeamId = tp.TeamId,
                    TeamName = tp.Team!.Name,
                    PlayerId = tp.PlayerId,
                    PlayerName = tp.Player!.Name,
                })
                .FirstOrDefaultAsync();

            if (teamPlayer == null)
            {
                return NotFound("Player not found in any team");
            }

            return Ok(teamPlayer);
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PutPlayerDto pl)
        {
            if (id != pl.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return BadRequest("Player not found");
            }

            var validationResponse = await _validationService.NameAndBirthDateExistsAsync<Player>(pl.Name, pl.Birth_date, pl.Id);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            if(await _validationService.FindByIdAsync<Position>(pl.PositionId) == null)
            {
                return BadRequest("Position not found");
            }

            player.Name = pl.Name;
            player.Birth_date = pl.Birth_date;
            player.PositionId = pl.PositionId;

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
        public async Task<ActionResult<GetPlayerDto>> PostPlayer(PostPlayerDto pl)
        {
            var validationResponse = await _validationService.NameAndBirthDateExistsAsync<Player>(pl.Name, pl.Birth_date);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            var position = await _validationService.FindByIdAsync<Position>(pl.PositionId);
            if (position == null)
            {
                return BadRequest("Position not found");
            }

            var player = new Player
            {
                Name = pl.Name,
                Birth_date = pl.Birth_date,
                PositionId = pl.PositionId,
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var returnPlayer = new GetPlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                Birth_date = player.Birth_date,
                PositionId = player.PositionId,
                PositionName = position.Name,
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
                return NotFound("Player not found");
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
