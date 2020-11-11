using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Lab1_Cats;

namespace Web_Lab1_Cats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Species1Controller : ControllerBase
    {
        private readonly CatsContext _context;

        public Species1Controller(CatsContext context)
        {
            _context = context;
        }

        // GET: api/Species1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Species>>> GetSpecies()
        {
            return await _context.Species.ToListAsync();
        }

        // GET: api/Species1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Species>> GetSpecies(int id)
        {
            var species = await _context.Species.FindAsync(id);

            if (species == null)
            {
                return NotFound();
            }

            return species;
        }

        // PUT: api/Species1/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecies(int id, Species species)
        {
            if (id != species.Id)
            {
                return BadRequest();
            }
            int counter = 0;
            foreach (var g in _context.Species)
            {
                if (g.Name == species.Name) { counter++; break; }
            }

            if (counter != 0)
            {
                ModelState.AddModelError("Spesies", "Така порода вже існує");
            }
            else { _context.Entry(species).State = EntityState.Modified; }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeciesExists(id))
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

        // POST: api/Species1
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Species>> PostSpecies(Species species)
        {
            int counter = 0;
            foreach (var g in _context.Species)
            {
                if (g.Name == species.Name) { counter++; break; }
            }

            if (counter != 0)
            {
                ModelState.AddModelError("Spesies", "Така порода вже існує");
            }
            else
            {
                _context.Species.Add(species);
                await _context.SaveChangesAsync();
            }
            
            return CreatedAtAction("GetSpecies", new { id = species.Id }, species);
        }

        // DELETE: api/Species1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Species>> DeleteSpecies(int id)
        {
            var species = await _context.Species.FindAsync(id);

            if (species == null)
            {
                return NotFound();
            }

            var cats = from cat in _context.Cats
                                  where cat.SpeciesId == id
                                  select cat;
            foreach (var cat in cats)
            {
                _context.Cats.Remove(cat);
            }

            _context.Species.Remove(species);
            await _context.SaveChangesAsync();

            return species;
        }

        private bool SpeciesExists(int id)
        {
            return _context.Species.Any(e => e.Id == id);
        }
    }
}
