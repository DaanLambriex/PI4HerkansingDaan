using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PI4Daan;
using PI4Daan.Data;

namespace PI4Daan.Controllers
{
    public class CollectiblesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CollectiblesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Collectibles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Collectibles.ToListAsync());
        }

        // GET: Collectibles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectible = await _context.Collectibles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectible == null)
            {
                return NotFound();
            }

            return View(collectible);
        }

        // GET: Collectibles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Collectibles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Value,Category,Brand,Percentage,Rating")] Collectible collectible)
        {
            if (ModelState.IsValid)
            {
                // Valideer de prijs en waarde op correct formaat
                collectible.Price = Math.Round(collectible.Price, 2);
                collectible.Value = Math.Round(collectible.Value, 2);
                collectible.Percentage = Math.Round(collectible.Percentage, 1);

                _context.Add(collectible);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collectible);
        }

        // GET: Collectibles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectible = await _context.Collectibles.FindAsync(id);
            if (collectible == null)
            {
                return NotFound();
            }
            return View(collectible);
        }

        // POST: Collectibles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Value,Category,Brand,Percentage,Rating")] Collectible collectible)
        {
            if (id != collectible.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                collectible.Percentage = Math.Round(collectible.Percentage, 1);

                try
                {
                    _context.Update(collectible);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectibleExists(collectible.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(collectible);
        }

        // GET: Collectibles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectible = await _context.Collectibles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectible == null)
            {
                return NotFound();
            }

            return View(collectible);
        }

        // POST: Collectibles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collectible = await _context.Collectibles.FindAsync(id);
            if (collectible != null)
            {
                _context.Collectibles.Remove(collectible);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectibleExists(int id)
        {
            return _context.Collectibles.Any(e => e.Id == id);
        }
    }
}
