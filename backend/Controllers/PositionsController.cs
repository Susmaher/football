using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Context;
using backend.Models;
using backend.Services;
using backend.Dtos.Position;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;

        public PositionsController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositions()
        {
            var positions = await _context.Positions
                .Select(p => new PositionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToListAsync();

            return Ok(positions);
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDto>> GetPosition(int id)
        {
            var position = await _context.Positions
                .Where(p => p.Id == id)
                .Select(p => new PositionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .FirstOrDefaultAsync();

            if (position == null)
            {
                return NotFound();
            }

            return Ok(position);
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionDto po)
        {
            if (id != po.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound("Position not found");
            }

            if(await _validationService.NameExistsAsync<Position>(po.Name, id))
            {
                return BadRequest("Position with this name already exists");
            }

            position.Name = po.Name;

            _context.Entry(position).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _validationService.FindByIdAsync<Position>(id) == null)
                {
                    return NotFound("Position not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PositionDto>> PostPosition(PostPositionDto po)
        {
            if(await _validationService.NameExistsAsync<Position>(po.Name))
            {
                return BadRequest("Position with this name already exists");
            }

            var position = new Position { Name = po.Name };

            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            var returnPosition = new PositionDto
            {
                Id = position.Id,
                Name = position.Name,
            };

            return CreatedAtAction("GetPosition", new { id = position.Id }, returnPosition);
        }

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound("Position not found");
            }

            var hasPlayer = await _context.Players.AnyAsync(p => p.PositionId == position.Id);
            if (hasPlayer) 
            {
                return BadRequest("This position cannot be deleted because it is associated with existing players.");
            }

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
