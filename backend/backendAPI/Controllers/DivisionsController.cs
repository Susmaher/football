using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Dtos.Division;
using backend.Services;
using backend.Dtos.Team;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;
        public DivisionsController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DivisionDto>>> GetDivisions()
        {
            var divisions = await _context.Divisions
                .Select(d => new DivisionDto
                {
                    Id = d.Id,
                    Name = d.Name,
                })
                .ToListAsync();

            return Ok(divisions);
        }

        // GET: api/Divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DivisionDto>> GetDivision(int id)
        { 
            var division = await _context.Divisions
                .Where(d => d.Id == id)
                .Select(d => new DivisionDto
                {
                    Id = d.Id,
                    Name = d.Name,
                })
                .FirstOrDefaultAsync();

            if (division == null)
            {
                return NotFound("Division not found");
            }

            return division;
        }

        // PUT: api/Divisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, DivisionDto divisiondto)
        {
            if (id != divisiondto.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
            {
                return NotFound("Division not found");
            }

            //validate whether name exists
            var validationResponse = await _validationService.NameExistsAsync<Division>(divisiondto.Name, divisiondto.Id);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            division.Name = divisiondto.Name;

            _context.Entry(division).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //it's needed if multiple user modify the database at once
                if (await _validationService.FindByIdAsync<Division>(id) == null)
                {
                    return NotFound("Division not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Divisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DivisionDto>> PostDivision(PostDivisionDto divisiondto)
        {
            var validationResponse = await _validationService.NameExistsAsync<Division>(divisiondto.Name);
            if (!validationResponse.Success)
            {
                return BadRequest(validationResponse.Message);
            }

            var division = new Division() 
            { 
                Name = divisiondto.Name
            };

            _context.Divisions.Add(division);
            await _context.SaveChangesAsync();

            var returnDivision = new DivisionDto
            {
                Id = division.Id,
                Name = division.Name
            };

            return CreatedAtAction("GetDivision", new { id = division.Id }, returnDivision);
        }

        // DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDivision(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
            {
                return NotFound("Division not found");
            }

            //since team must have a division, a division cannot be deleted if a team is in that
            var hasTeams = await _context.Teams.AnyAsync(t=> t.DivisionId == id);
            if (hasTeams)
            {
                return BadRequest("Division cannot be deleted because it is associated with existing teams.");
            }

            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
