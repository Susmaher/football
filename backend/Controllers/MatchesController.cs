using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
                    MatchDate = m.MatchDate,
                    HomeScore = m.Status == MatchStatus.Played ? m.HomeScore : null,
                    AwayScore = m.Status == MatchStatus.Played ? m.AwayScore : null,
                    Round = m.Round,
                    Status = m.Status.ToString(),
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam!.Name,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam!.Name,
                    DivisionId = m.DivisionId,
                    DivisionName = m.Division!.Name,
                    RefereeId = m.Referee!.Id,
                    RefereeName = m.Referee!.Name,
                    FieldId = m.Field!.Id,
                    FieldName = m.Field!.Name,
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
                    MatchDate = m.MatchDate,
                    HomeScore = m.Status == MatchStatus.Played ? m.HomeScore : null,
                    AwayScore = m.Status == MatchStatus.Played ? m.AwayScore : null,
                    Round = m.Round,
                    Status = m.Status.ToString(),
                    HomeTeamId = m.HomeTeamId,
                    HomeTeamName = m.HomeTeam!.Name,
                    AwayTeamId = m.AwayTeamId,
                    AwayTeamName = m.AwayTeam!.Name,
                    DivisionId = m.DivisionId,
                    DivisionName = m.Division!.Name,
                    RefereeId = m.Referee!.Id,
                    RefereeName = m.Referee!.Name,
                    FieldId = m.Field!.Id,
                    FieldName = m.Field!.Name,
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

            match.MatchDate = ma.MatchDate;
            match.HomeScore = ma.HomeScore;
            match.AwayScore = ma.AwayScore;
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
                MatchDate = ma.MatchDate,
                HomeScore = ma.HomeScore,
                AwayScore = ma.AwayScore,
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
                MatchDate = match.MatchDate,
                HomeScore = validationResult.data!.Status == MatchStatus.Played ? match.HomeScore : null,
                AwayScore = validationResult.data!.Status == MatchStatus.Played ? match.AwayScore : null,
                Round = match.Round,
                Status = validationResult.data!.Status.ToString(),
                HomeTeamId = match.HomeTeamId,
                HomeTeamName = validationResult.data.HomeTeam!.Name,
                AwayTeamId = match.AwayTeamId,
                AwayTeamName = validationResult.data.AwayTeam!.Name,
                DivisionId = match.DivisionId,
                DivisionName = validationResult.data.Division!.Name,
                RefereeId = validationResult.data!.Referee!.Id,
                RefereeName = validationResult.data!.Referee!.Name,
                FieldId = validationResult.data!.Field!.Id,
                FieldName = validationResult.data!.Field!.Name,
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
        [HttpGet("draw/preview/{division}")]
        public async Task<ActionResult<IEnumerable<GetMatchDto>>> DrawMatches(int division)
        {
            if(await _commonValidation.FindByIdAsync<Division>(division) == null)
            {
                return NotFound("Divison not found");
            }
            var teams = await _context.Teams.Where(t => t.DivisionId == division).Include(t => t.Field).ToListAsync();

            var home_matches = new List<GetMatchDto>();

            //double round-robin algorithm
            //rounds: teams%2==0 ? teams.Count()-1 : teams.Count()
            int rounds = teams.Count()-1;
            if (teams.Count() % 2 != 0)
            {
                teams.Add(new Team { Id = 0, Name="DummyBot", Points = -1, DivisionId = -1, FieldId = -1 });
                rounds++;
            }

            Random rndmain = new();
            int n = teams.Count();
            while (n > 1)
            {
                n--;
                int k = rndmain.Next(n + 1);
                var value = teams[k];
                teams[k] = teams[n];
                teams[n] = value;
            }

            var referees = await _context.Referees.ToListAsync();

            var team_count = teams.Count();
            for (int i = 1; i <= rounds; i++)
            {
                int t_max = team_count-1;
                for (int j = 0; j < team_count / 2; j++)
                {
                    var first_team_match_count = home_matches.Where(t => t.HomeTeamId == teams[j].Id).Count();
                    var second_team_match_count = home_matches.Where(t => t.HomeTeamId == teams[t_max].Id).Count();
                    var rand = new Random();
                    var referee = referees[rand.Next(referees.Count())];
                    var home_match = new GetMatchDto
                    {
                        Round = i,
                        Status = "Scheduled",
                        DivisionId = division,
                        HomeTeamId = first_team_match_count <= second_team_match_count ? teams[j].Id : teams[t_max].Id,
                        HomeTeamName = first_team_match_count <= second_team_match_count ? teams[j].Name : teams[t_max].Name,
                        AwayTeamId = first_team_match_count <= second_team_match_count ? teams[t_max].Id : teams[j].Id,
                        AwayTeamName = first_team_match_count <= second_team_match_count ? teams[t_max].Name : teams[j].Name,
                        RefereeId = referee.Id,
                        RefereeName = referee.Name,
                    };
                    t_max--;
                    if(home_match.HomeTeamId == 0 || home_match.AwayTeamId == 0)
                    {
                        continue;
                    }
                    home_match.FieldName = teams.Where(t => t.Id == home_match.HomeTeamId).Select(t => t.Field!.Name).FirstOrDefault()!;

                    home_matches.Add(home_match);
                }

                var temp = teams[team_count - 1];
                for (int j = team_count - 1; j > 0; j--)
                {
                    teams[j] = teams[j - 1];
                }
                teams[1] = temp;
            }

            //double part
            var away_matches = new List<GetMatchDto>();
            foreach (var match in home_matches)
            {
                var rand = new Random();
                var referee = referees[rand.Next(referees.Count())];
                var away_match = new GetMatchDto
                {
                    Round = match.Round+rounds,
                    Status = "Scheduled",
                    DivisionId = division,
                    HomeTeamId = match.AwayTeamId,
                    HomeTeamName = match.AwayTeamName,
                    AwayTeamId = match.HomeTeamId,
                    AwayTeamName = match.HomeTeamName,
                    RefereeId = referee.Id,
                    RefereeName = referee.Name,
                    FieldName = teams.FirstOrDefault(t=>t.Id == match.AwayTeamId)!.Field!.Name,
                };
                away_matches.Add(away_match);
            }

            
            List<GetMatchDto> matches = [..home_matches, ..away_matches];

            return Ok(matches);
        }
    }
}
