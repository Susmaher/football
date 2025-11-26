using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Context;
using backend.Dtos.Field;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly FootballDbContext _context;
        private readonly ICommonValidationService _validationService;
        public FieldsController(FootballDbContext context, ICommonValidationService validationService)
        {
            _context = context;
            _validationService = validationService;
        }

        // GET: api/Fields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldDto>>> GetFields()
        {
            var fields = await _context.Fields
                .Select(f => new FieldDto
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .ToListAsync();

            return Ok(fields);
        }

        // GET: api/Fields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldDto>> GetField(int id)
        {
            var field = await _context.Fields
                .Where(f=>f.Id==id)
                .Select(f=>new FieldDto
                {
                    Id = f.Id,
                    Name = f.Name,
                })
                .FirstOrDefaultAsync();

            if (field == null)
            {
                return NotFound("Field doesn't exists with this ID");
            }

            return Ok(field);
        }

        // PUT: api/Fields/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutField(int id, FieldDto fl)
        {
            if (id != fl.Id)
            {
                return BadRequest("Route ID and body ID do not match");
            }

            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return BadRequest("Field not found");
            }
            
            if(await _validationService.NameExistsAsync<Field>(fl.Name, id))
            {
                return BadRequest("Field this with name already exists");
            }

            field.Name = fl.Name;

            _context.Entry(field).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _validationService.FindByIdAsync<Field>(id) == null)
                {
                    return NotFound("Field not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Fields
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FieldDto>> PostField(PostFieldDto fl)
        {
            if(await _validationService.NameExistsAsync<Field>(fl.Name))
            {
                return BadRequest("Field with this name already exists");
            }

            var field = new Field
            {
                Name = fl.Name,
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            var returnField = new FieldDto
            {
                Id = field.Id,
                Name = fl.Name,
            };

            return CreatedAtAction("GetField", new { id = field.Id }, returnField);
        }

        // DELETE: api/Fields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteField(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound("Field not found");
            }

            var hasMatches = await _context.Matches.AnyAsync(m=>m.FieldId == id);
            if (hasMatches)
            {
                return BadRequest("This field cannot be deleted because it is associated with existing matches.");
            }

            var hasTeams = await _context.Teams.AnyAsync(t=>t.FieldId==id);
            if (hasTeams) 
            {
                return BadRequest("This field cannot be deleted because it is associated with existing teams.");
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
