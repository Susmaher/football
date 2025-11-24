using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.Event;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;

        public EventsController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                })
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var evn = await _context.Events
                .Where(e=>e.Id==id)
                .Select(e=> new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                })
                .FirstOrDefaultAsync();

            if (evn == null)
            {
                return NotFound("Event not found");
            }

            return Ok(evn);
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, EventDto evn)
        {
            if (id != evn.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            if(await _validationService.NameExistsAsync<Event>(evn.Name, evn.Id))
            {
                return BadRequest("Event with this name already exists");
            }

            var @event = new Event
            {
                Id = evn.Id,
                Name = evn.Name,
            };
            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _validationService.FindByIdAsync<Event>(id) == null)
                {
                    return NotFound("Event not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Events
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent(PostEventDto evn)
        {
            if(await _validationService.NameExistsAsync<Event>(evn.Name))
            {
                return BadRequest("Event with this name already exists");
            }

            var @event = new Event
            {
                Name = evn.Name,
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            var returnEvent = new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
            };

            return CreatedAtAction("GetEvent", new { id = @event.Id }, returnEvent);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound("Event not found");
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
