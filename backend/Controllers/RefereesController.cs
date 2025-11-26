using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.Referee;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefereesController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;
        public RefereesController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Referees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefereeDto>>> GetReferees()
        {
            var referees = await _context.Referees
                .Select(r => new RefereeDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Birth_date = r.Birth_date,
                })
                .ToListAsync();
            return Ok(referees);
        }

        // GET: api/Referees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RefereeDto>> GetReferee(int id)
        {
            var referee = await _context.Referees
                .Where(r=> r.Id == id)
                .Select(r=> new RefereeDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Birth_date = r.Birth_date,
                })
                .FirstOrDefaultAsync();

            if (referee == null)
            {
                return NotFound("Referee doesn't exist with this ID");
            }

            return Ok(referee);
        }

        // PUT: api/Referees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReferee(int id, RefereeDto rf)
        {
            if (id != rf.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var referee = await _context.Referees.FindAsync(id);
            if (referee == null)
            {
                return BadRequest("Referee not found");
            }

            var validationResponse = await _validationService.NameAndBirthDateExistsAsync<Referee>(rf.Name, rf.Birth_date, rf.Id);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            referee.Name = rf.Name;
            referee.Birth_date = rf.Birth_date;

            _context.Entry(referee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _validationService.FindByIdAsync<Referee>(id) == null)
                {
                    return NotFound("Referee not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Referees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RefereeDto>> PostReferee(PostRefereeDto rf)
        {
            var validationResponse = await _validationService.NameAndBirthDateExistsAsync<Referee>(rf.Name, rf.Birth_date);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            var referee = new Referee 
            { 
                Name = rf.Name,
                Birth_date = rf.Birth_date,
            };

            _context.Referees.Add(referee);
            await _context.SaveChangesAsync();

            var returnReferee = new RefereeDto
            {
                Id = referee.Id,
                Name = referee.Name,
                Birth_date = referee.Birth_date,
            };

            return CreatedAtAction("GetReferee", new { id = referee.Id }, returnReferee);
        }

        // DELETE: api/Referees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReferee(int id)
        {
            var referee = await _context.Referees.FindAsync(id);
            if (referee == null)
            {
                return NotFound("Referee not found");
            }

            var hasMatches = await _context.Matches.AnyAsync(m => m.RefereeId == id);
            if (hasMatches) 
            {
                return BadRequest("This referee cannot be deleted because it is associated with existing matches.");
            }

            _context.Referees.Remove(referee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
