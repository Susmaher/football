using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.Team;
using Microsoft.OpenApi.Validations;
using System.Security.Policy;
using backend.Dtos.TeamPlayer;
using backend.Services;
using backend.Services.TeamsValidation;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _commonValidationService;
        private readonly ITeamValidationService _teamValidationService;
        public TeamsController(FootballDbContext context, ITeamValidationService teamValidationService, ICommonValidationService commonValidationService)
        {
            _context = context;
            _teamValidationService = teamValidationService;
            _commonValidationService = commonValidationService;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTeamsDto>>> GetTeams()
        {
            var teams = await _context.Teams
                .Select(t => new GetTeamsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Points = t.Points,
                    DivisionId = t.DivisionId,
                    DivisionName = t.Division!.Name,
                    FieldId = t.FieldId,
                    FieldName = t.Field!.Name,
                })
                .ToListAsync();
            return Ok(teams);
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTeamsDto>> GetTeam(int id)
        {
            var team = await _context.Teams
                .Where(t=>t.Id==id)
                .Select(t=> new GetTeamsDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Points = t.Points,
                    DivisionId = t.DivisionId,
                    DivisionName = t.Division!.Name,
                    FieldId = t.FieldId,
                    FieldName = t.Field!.Name,
                })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return NotFound("Team not found");
            }

            return Ok(team);
        }

        // GET: api/Teams/5/players
        [HttpGet("{id}/players")]
        public async Task<ActionResult<IEnumerable<GetTeamPlayerDto>>> GetTeamPlayersInTeam(int id)
        {
            if(await _commonValidationService.FindByIdAsync<Team>(id) == null)
            {
                return NotFound("Team not found");
            }

            var teamPlayers = await _context.TeamPlayers
                .Where(tp=>tp.TeamId==id)
                .Select(tp => new GetTeamPlayerDto
                {
                    Id = tp.Id,
                    TeamId = tp.TeamId,
                    TeamName = tp.Team!.Name,
                    PlayerId = tp.PlayerId,
                    PlayerName = tp.Player!.Name
                })
                .ToListAsync();

            return Ok(teamPlayers);
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, PutTeamDto teamdto)
        {
            if (id != teamdto.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("Team not found");
            }

            var validationResult = await _teamValidationService.ValidateTeamUpdateAsync(teamdto);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }


            team.Name = teamdto.Name;
            team.Points = teamdto.Points;
            team.DivisionId = teamdto.DivisionId;
            team.FieldId = teamdto.FieldId;

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _commonValidationService.FindByIdAsync<Team>(id) == null)
                {
                    return NotFound("Team not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // PATCH: api/Teams/5/points
        [HttpPatch("{id}/points")]
        public async Task<IActionResult> PatchTeamScore(int id, ChangeTeamScoreDto teamdto)
        {
            if (id != teamdto.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("Team not found");
            }

            team.Points += teamdto.Points;
            if (team.Points < 0)
            {
                return BadRequest("A team cannot have negative points");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _commonValidationService.FindByIdAsync<Team>(id) == null)
                {
                    return NotFound("Team not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetTeamsDto>> PostTeam(CreateTeamDto tm)
        {
            var validationResult = await _teamValidationService.ValidateTeamCreationAsync(tm);
            if (!validationResult.Success) 
            {
                return BadRequest(validationResult.Message);
            }

            var team = new Team
            {
                Name=tm.Name,
                Points = tm.Points ?? 0,
                DivisionId = tm.DivisionId,
                FieldId = tm.FieldId,
            }; 

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            var returnTeamDto = new GetTeamsDto
            {
                Id = team.Id,
                Name = team.Name,
                Points = team.Points,
                DivisionId = team.DivisionId,
                DivisionName = validationResult.data!.Division.Name,
                FieldId = team.FieldId,
                FieldName = validationResult.data!.Field.Name,
            };

            return CreatedAtAction("GetTeam", new { id = team.Id }, returnTeamDto);
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound("Team not found");
            }

            //if players are attached to teams, then the teamplayers will be removed
            var teamPlayers = await _context.TeamPlayers
                .Where(tp => tp.TeamId == id)
                .ToListAsync();
            if (teamPlayers.Any())
            {
                _context.TeamPlayers.RemoveRange(teamPlayers);
            }

            //if a team has matches, team cannot be deleted (in order to avoid errors in matches)
            var hasMatches = await _context.Matches.AnyAsync(m => m.HomeTeamId == id || m.AwayTeamId == id);
            if (hasMatches)
            {
                return BadRequest("This team cannot be deleted because it is associated with existing matches.");
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
