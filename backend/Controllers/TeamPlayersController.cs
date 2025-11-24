using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.TeamPlayer;
using backend.Services;
using backend.Services.TeamPlayerValidation;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamPlayersController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _commonValidation;
        private readonly ITeamPlayerValidationService _teamPlayerValidation;
        public TeamPlayersController(FootballDbContext context, ICommonValidationService commonValidation, ITeamPlayerValidationService teamPlayerValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
            _teamPlayerValidation = teamPlayerValidation;
        }

        // GET: api/TeamPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTeamPlayerDto>>> GetTeamPlayers()
        {
            var teamplayer = await _context.TeamPlayers
                .Select(tp => new GetTeamPlayerDto
                {
                    Id = tp.Id,
                    TeamId = tp.TeamId,
                    TeamName = tp.Team!.Name,
                    PlayerId = tp.PlayerId,
                    PlayerName = tp.Player!.Name
                })
                .ToListAsync();
            return Ok(teamplayer);
        }

        // GET: api/TeamPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTeamPlayerDto>> GetTeamPlayer(int id)
        {
            var teamPlayer = await _context.TeamPlayers
                .Where(tp=>tp.Id==id)
                .Select(tp=>new GetTeamPlayerDto
                {
                    Id = tp.Id,
                    TeamId = tp.TeamId,
                    TeamName = tp.Team!.Name,
                    PlayerId = tp.PlayerId,
                    PlayerName = tp.Player!.Name
                })
                .FirstOrDefaultAsync();

            if (teamPlayer == null)
            {
                return NotFound();
            }

            return teamPlayer;
        }

        // PUT: api/TeamPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamPlayer(int id, PutTeamPlayerDto tmpl)
        {
            if (id != tmpl.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var teamPlayer = await _context.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            var validationResult = await _teamPlayerValidation.ValidateTeamPlayerUpdateAsync(tmpl);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }


            teamPlayer.TeamId = tmpl.TeamId;
            teamPlayer.PlayerId = tmpl.PlayerId;

            _context.Entry(teamPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _commonValidation.FindByIdAsync<TeamPlayer>(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TeamPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetTeamPlayerDto>> PostTeamPlayer(PostTeamPlayerDto teampl)
        {
            var validationResult = await _teamPlayerValidation.ValidateTeamPlayerCreationAsync(teampl);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }

            var teamPlayer = new TeamPlayer
            {
                PlayerId = teampl.PlayerId,
                TeamId = teampl.TeamId,
            };

            _context.TeamPlayers.Add(teamPlayer);
            await _context.SaveChangesAsync();

            var returnPlayer = new GetTeamPlayerDto
            {
                Id = teamPlayer.Id,
                PlayerId = teamPlayer.PlayerId,
                PlayerName = validationResult.data!.Player.Name,
                TeamId = teamPlayer.TeamId,
                TeamName = validationResult.data!.Team.Name
            };

            return CreatedAtAction("GetTeamPlayer", new { id = teamPlayer.Id }, returnPlayer);
        }

        // DELETE: api/TeamPlayers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamPlayer(int id)
        {
            var teamPlayer = await _context.TeamPlayers.FindAsync(id);
            if (teamPlayer == null)
            {
                return NotFound();
            }

            //if a player has matchevent, it cannot be deleted
            var hasMatchEvents = await _context.MatchEvents.AnyAsync(me => me.TeamPlayerId == teamPlayer.Id);
            if (hasMatchEvents) 
            {
                return BadRequest("This teamplayer cannot be deleted because it is associated with existing match events.");
            }

            _context.TeamPlayers.Remove(teamPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
