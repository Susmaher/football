using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.MatchEvent;
using backend.Services;
using backend.Services.MatchEventValidation;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchEventsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _commonValidation;
        private readonly IMatchEventValidationService _matchEventValidation;
        public MatchEventsController(FootballDbContext context, ICommonValidationService commonValidation, IMatchEventValidationService matchEventValidation)
        {
            _context = context;
            _commonValidation = commonValidation;
            _matchEventValidation = matchEventValidation;
        }

        // GET: api/MatchEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMatchEventDto>>> GetMatchEvents()
        {
            var matchEvents = await _context.MatchEvents
                .Include(x => x.TeamPlayer)
                    .ThenInclude(tp => tp!.Player)
                .Select(me => new GetMatchEventDto
                {
                    Id = me.Id,
                    MatchId = me.MatchId,
                    TeamId = me.TeamId,
                    TeamName = me.Team!.Name,
                    TeamPlayerId = me.TeamPlayerId,
                    TeamPlayerName = me.TeamPlayer!.Player!.Name,
                    EventType = me.EventType.ToString(),
                    Minute = me.Minute,
                    ExtraMinute = me.ExtraMinute ?? 0,
                }).ToListAsync();

            return Ok(matchEvents);
        }

        // GET: api/MatchEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMatchEventDto>> GetMatchEvent(int id)
        {
            var matchEvent = await _context.MatchEvents
                .Where(me => me.Id == id)
                .Include(x => x.TeamPlayer)
                    .ThenInclude(tp => tp!.Player)
                .Select(me => new GetMatchEventDto
                {
                    Id = me.Id,
                    MatchId = me.MatchId,
                    TeamId = me.TeamId,
                    TeamName = me.Team!.Name,
                    TeamPlayerId = me.TeamPlayerId,
                    TeamPlayerName = me.TeamPlayer!.Player!.Name,
                    EventType = me.EventType.ToString(),
                    Minute = me.Minute,
                    ExtraMinute = me.ExtraMinute ?? 0,
                }).FirstOrDefaultAsync();

            if (matchEvent == null)
            {
                return NotFound();
            }

            return Ok(matchEvent);
        }

        // PUT: api/MatchEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatchEvent(int id, PutMatchEventDto me)
        {
            if (id != me.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var matchEvent = await _context.MatchEvents.FindAsync(id);
            if(matchEvent == null)
            {
                return NotFound("Match event not found");
            }

            var validationResult = await _matchEventValidation.ValidateMatchEventUpdateAsync(me);
            if (!validationResult.Success) 
            {
                return BadRequest(validationResult.Message);
            }

            matchEvent.EventType = validationResult.data!.EventType;
            matchEvent.TeamId = me.TeamId;
            matchEvent.TeamPlayerId = me.TeamPlayerId;
            matchEvent.MatchId = me.MatchId;
            matchEvent.Minute = me.Minute;
            matchEvent.ExtraMinute = me.ExtraMinute ?? null;

            _context.Entry(matchEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _commonValidation.FindByIdAsync<MatchEvent>(id) == null)
                {
                    return NotFound("Match event not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MatchEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetMatchEventDto>> PostMatchEvent(PostMatchEventDto me)
        {
            var validationResult = await _matchEventValidation.ValidateMatchEventCreationAsync(me);
            if (!validationResult.Success)
            {
                return BadRequest(validationResult.Message);
            }

            var matchEvent = new MatchEvent
            {
                MatchId = me.MatchId,
                TeamId = me.TeamId,
                TeamPlayerId = me.TeamPlayerId,
                EventType = validationResult.data!.EventType,
                Minute = me.Minute,
                ExtraMinute = me.ExtraMinute ?? null,
            };

            _context.MatchEvents.Add(matchEvent);
            await _context.SaveChangesAsync();

            var returnMatchEvent = new GetMatchEventDto
            {
                Id = matchEvent.Id,
                MatchId = matchEvent.MatchId,
                TeamId = matchEvent.TeamId,
                TeamName = validationResult.data!.Team!.Name,
                TeamPlayerId = matchEvent.TeamPlayerId,
                TeamPlayerName = validationResult.data.Player!.Name,
                EventType = me.EventType,
                Minute = matchEvent.Minute,
                ExtraMinute = matchEvent.ExtraMinute ?? 0,
            };

            return CreatedAtAction("GetMatchEvent", new { id = matchEvent.Id }, returnMatchEvent);
        }

        // DELETE: api/MatchEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatchEvent(int id)
        {
            var matchEvent = await _context.MatchEvents.FindAsync(id);
            if (matchEvent == null)
            {
                return NotFound("Match event not found");
            }

            _context.MatchEvents.Remove(matchEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
