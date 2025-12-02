using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using backend.Context;
using backend.Dtos.Match;
using backend.Models;
using backend.Services;
using backend.Services.MatchValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _commonValidation;
        private readonly IMatchValidationService _matchValidation;
        public MatchesController(FootballDbContext context, ICommonValidationService commonValidation, IMatchValidationService matchValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
            _matchValidation = matchValidation;
        }

        //GET: api/Matches?played=true
        //GET: api/Matches?played=false
        //GET: api/Matches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMatchDto>>> GetMatches([FromQuery] bool? played = null, [FromQuery] int? divison = null, [FromQuery] int? round = null)
        {
            var query = _context.Matches.AsQueryable();

            if(played == true)
            {
                query = query.Where(m => m.Status == MatchStatus.Played);
            } else if (played == false)
            {
                query = query.Where(m => m.Status != MatchStatus.Played);
            }

            if (divison.HasValue)
            {
                if(await _commonValidation.FindByIdAsync<Division>(divison.Value) == null)
                {
                    return BadRequest("Division not found");
                }
                query = query.Where(m => m.DivisionId == divison.Value);
            }

            if (round.HasValue)
            {
                query = query.Where(m => m.Round == round.Value);
            }

            var matches = await query
                .Select(m => new GetMatchDto
                {
                    Id = m.Id,
                    Match_date = m.Match_date,
                    Home_score = m.Status == MatchStatus.Played ? m.Home_score : null,
                    Away_score = m.Status == MatchStatus.Played ? m.Away_score : null,
                    Round = m.Round,
                    Status = m.Status.ToString(),
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam!.Name,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam!.Name,
                    DivisionId = m.DivisionId,
                    DivisionName = m.Division!.Name,
                    RefereeName = m.Status == MatchStatus.Played ? m.Referee!.Name : null,
                    FieldName = m.Status == MatchStatus.Played ? m.Field!.Name : null,
                }).ToListAsync();

            return Ok(matches);
        }

        // GET: api/Matches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMatchDto>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Where(m => m.Id == id)
                .Select(m => new GetMatchDto
                {
                    Id = m.Id,
                    Match_date = m.Match_date,
                    Home_score = m.Status == MatchStatus.Played ? m.Home_score : null,
                    Away_score = m.Status == MatchStatus.Played ? m.Away_score : null,
                    Round = m.Round,
                    Status = m.Status.ToString(),
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam!.Name,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam!.Name,
                    DivisionId = m.DivisionId,
                    DivisionName = m.Division!.Name,
                    RefereeName = m.Status == MatchStatus.Played ? m.Referee!.Name : null,
                    FieldName = m.Status == MatchStatus.Played ? m.Field!.Name : null,
                }).FirstOrDefaultAsync();

            if (match == null)
            {
                return NotFound("Match not found");
            }

            return Ok(match);
        }

        // PUT: api/Matches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, PutMatchDto ma)
        {
            if (id != ma.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var match = await _commonValidation.FindByIdAsync<Match>(id);
            if (match == null) 
            {
                return NotFound("Match not found");
            }

            var validationResult = await _matchValidation.ValidateMatchUpdateAsync(ma);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }

            match.Match_date = ma.Match_date;
            match.Home_score = ma.Home_score;
            match.Away_score = ma.Away_score;
            match.Status = validationResult.data!.Status;
            match.Round = ma.Round;
            match.HomeTeamId = ma.HomeTeamId;
            match.AwayTeamId = ma.AwayTeamId;
            match.DivisionId = ma.DivisionId;
            match.RefereeId = ma.RefereeId;
            match.FieldId = ma.FieldId;

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _commonValidation.FindByIdAsync<Match>(id) == null)
                {
                    return NotFound("Match not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Matches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetMatchDto>> PostMatch(PostMatchDto ma)
        {
            var validationResult = await _matchValidation.ValidateMatchCreationAsync(ma);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }

            var match = new Match
            {
                Match_date = ma.Match_date,
                Home_score = ma.Home_score,
                Away_score = ma.Away_score,
                Round = ma.Round,
                Status = validationResult.data!.Status,
                HomeTeamId = ma.HomeTeamId,
                AwayTeamId = ma.AwayTeamId,
                DivisionId = ma.DivisionId,
                RefereeId = ma.RefereeId,
                FieldId = ma.FieldId,
            };

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            var returnMatch = new GetMatchDto {
                Id = match.Id,
                Match_date = match.Match_date,
                Home_score = validationResult.data!.Status == MatchStatus.Played ? match.Home_score : null,
                Away_score = validationResult.data!.Status == MatchStatus.Played ? match.Away_score : null,
                Round = match.Round,
                Status = validationResult.data!.Status.ToString(),
                HomeTeamId = match.HomeTeamId,
                HomeTeamName = validationResult.data.HomeTeam!.Name,
                AwayTeamId = match.AwayTeamId,
                AwayTeamName = validationResult.data.AwayTeam!.Name,
                DivisionId = match.DivisionId,
                DivisionName = validationResult.data.Division!.Name,
                RefereeName = validationResult.data!.Status == MatchStatus.Played ? validationResult.data.Referee!.Name : null,
                FieldName = validationResult.data!.Status == MatchStatus.Played ? validationResult.data.Field!.Name : null,
            };

            return CreatedAtAction("GetMatch", new { id = match.Id }, returnMatch);
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound("Match not found");
            }

            var matchEvents = await _context.MatchEvents
                .Where(me => me.MatchId == id)
                .ToListAsync();

            if (matchEvents.Any())
            {
                _context.MatchEvents.RemoveRange(matchEvents);
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }





        //---------------------------------------------------------------------------------------------------------------------------//
        // GET: draw
        [HttpGet("preview/{division}")]
        public async Task<ActionResult<IEnumerable<GetMatchDto>>> DrawMatches(int division)
        {
            var teams = await _context.Teams.Where(t=>t.DivisionId == division).ToListAsync();
            var team_count = teams.Count();
            var matches = new List<GetMatchDto>();

            if (team_count % 2 == 0)
            {
                teams.Add(new Team { Id = 0, Name="DummyBot", Points = -1, DivisionId = -1, FieldId = -1 }); 
            }


            int rounds = team_count % 2 == 0 ? team_count : team_count + 1;
            for (int i = 1; i < rounds; i++)
            {
                int t_max = team_count;
                for (int j = 0; j < team_count / 2; j++)
                {
                    var match = new GetMatchDto
                    {
                        Round = i,
                        Status = "Scheduled",
                        DivisionId = division,
                        HomeTeamId = teams[j].Id,
                        HomeTeamName = teams[j].Name,
                        AwayTeamId = teams[t_max - 1].Id,
                        AwayTeamName = teams[t_max - 1].Name,
                    };
                    t_max--;
                    if(match.HomeTeamId == 0 || match.AwayTeamId == 0)
                    {
                        continue;
                    }
                    matches.Add(match);
                }

                var temp = teams[team_count - 1];
                for (int j = team_count - 1; j > 0; j--)
                {
                    teams[j] = teams[j - 1];
                }
                teams[1] = temp;
            }

            return Ok(matches);
        }
    }
}
