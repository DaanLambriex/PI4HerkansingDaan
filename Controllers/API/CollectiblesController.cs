using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PI4Daan.Data;
using PI4Daan.Models;

namespace PI4Daan.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectiblesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CollectiblesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Collectibles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collectible>>> GetCollectibles()
        {
            return await _context.Collectibles.ToListAsync();
        }

        // GET: api/Collectibles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Collectible>> GetCollectible(int id)
        {
            var collectible = await _context.Collectibles.FindAsync(id);

            if (collectible == null)
            {
                return NotFound();
            }

            return collectible;
        }

        // PUT: api/Collectibles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCollectible(int id, [FromBody] Collectible collectible)
        {
            // Controleer of het id in de URL overeenkomt met het id in de body
            if (id != collectible.Id)
            {
                return BadRequest();
            }

            // Haal het bestaande Collectible object uit de database op
            var existingCollectible = await _context.Collectibles.FindAsync(id);
            if (existingCollectible == null)
            {
                return NotFound();
            }

            // Update alleen de velden die zijn meegegeven
            existingCollectible.Name = collectible.Name;
            existingCollectible.Description = collectible.Description;
            existingCollectible.Price = collectible.Price;
            existingCollectible.Value = collectible.Value;
            existingCollectible.CategoryId = collectible.CategoryId;
            existingCollectible.BrandId = collectible.BrandId;
            existingCollectible.Percentage = collectible.Percentage;
            existingCollectible.Rating = collectible.Rating;

            try
            {
                await _context.SaveChangesAsync(); // Sla wijzigingen op
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectibleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Geef foutmelding als er een andere databasefout is
                }
            }

            return NoContent(); // Stuur 204 No Content als de update succesvol was
        }


        // POST: api/Collectibles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collectible>> PostCollectible(Collectible collectible)
        {
            _context.Collectibles.Add(collectible);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollectible", new { id = collectible.Id }, collectible);
        }

        // DELETE: api/Collectibles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollectible(int id)
        {
            var collectible = await _context.Collectibles.FindAsync(id);
            if (collectible == null)
            {
                return NotFound();
            }

            _context.Collectibles.Remove(collectible);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CollectibleExists(int id)
        {
            return _context.Collectibles.Any(e => e.Id == id);
        }
    }
}
