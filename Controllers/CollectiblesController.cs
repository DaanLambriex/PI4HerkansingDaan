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
            ViewBag.Categories = _context.Collectibles.Select(c => c.Category).Distinct().ToList();
            ViewBag.Brands = _context.Collectibles.Select(c => c.Brand).Distinct().ToList();
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
            ViewBag.Categories = _context.Collectibles.Select(c => c.Category).Distinct().ToList();
            ViewBag.Brands = _context.Collectibles.Select(c => c.Brand).Distinct().ToList();
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

        // GET: Collectibles/List
        public async Task<IActionResult> List(
            string searchQuery,
            List<string> categories,
            List<string> brands,
            decimal? minPrice,
            decimal? maxPrice,
            decimal? minValue,
            decimal? maxValue,
            decimal? minPercentage,
            decimal? maxPercentage,
            double? minRating,
            double? maxRating)
        {
            // Start met alle collectibles
            var collectibles = _context.Collectibles.AsQueryable();

            // Zoekfilter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collectibles = collectibles.Where(c => c.Name.Contains(searchQuery) || c.Description.Contains(searchQuery));
            }

            // Categorie filter
            if (categories != null && categories.Any())
            {
                collectibles = collectibles.Where(c => categories.Contains(c.Category));
            }

            // Merk filter
            if (brands != null && brands.Any())
            {
                collectibles = collectibles.Where(c => brands.Contains(c.Brand));
            }

            // Prijsbereik filter
            if (minPrice.HasValue)
            {
                collectibles = collectibles.Where(c => c.Price >= minPrice.Value);
                ViewData["minPrice"] = minPrice;
            }
            if (maxPrice.HasValue)
            {
                collectibles = collectibles.Where(c => c.Price <= maxPrice.Value);
                ViewData["maxPrice"] = maxPrice;
            }

            // Waardebereik filter
            if (minValue.HasValue)
            {
                collectibles = collectibles.Where(c => c.Value >= minValue.Value);
                ViewData["minValue"] = minValue;
            }
            if (maxValue.HasValue)
            {
                collectibles = collectibles.Where(c => c.Value <= maxValue.Value);
                ViewData["maxValue"] = maxValue;
            }

            // Percentagebereik filter
            if (minPercentage.HasValue)
            {
                collectibles = collectibles.Where(c => c.Percentage >= minPercentage.Value);
                ViewData["minPercentage"] = minPercentage;
            }
            if (maxPercentage.HasValue)
            {
                collectibles = collectibles.Where(c => c.Percentage <= maxPercentage.Value);
                ViewData["maxPercentage"] = maxPercentage;
            }

            // Beoordeling filter
            if (minRating.HasValue)
            {
                collectibles = collectibles.Where(c => c.Rating >= minRating.Value);
                ViewData["minRating"] = minRating;
            }
            if (maxRating.HasValue)
            {
                collectibles = collectibles.Where(c => c.Rating <= maxRating.Value);
                ViewData["maxRating"] = maxRating;
            }


            // Haal gefilterde data op
            var filteredCollectibles = await collectibles.ToListAsync();

            // Bereken de totale prijs en waarde
            ViewBag.TotalPrice = filteredCollectibles.Sum(c => c.Price);
            ViewBag.TotalValue = filteredCollectibles.Sum(c => c.Value);

            // Categorieën en merken doorgeven aan de View
            ViewBag.Categories = await _context.Collectibles
                                               .Select(c => c.Category)
                                               .Distinct()
                                               .ToListAsync();
            ViewBag.Brands = await _context.Collectibles
                                           .Select(c => c.Brand)
                                           .Distinct()
                                           .ToListAsync();

            return View(await collectibles.ToListAsync());
        }

        public IActionResult ManageCategories()
        {
            var categories = _context.Collectibles.Select(c => c.Category).Distinct().ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(string newCategory)
        {
            if (!string.IsNullOrWhiteSpace(newCategory) && !_context.Collectibles.Any(c => c.Category == newCategory))
            {
                _context.Collectibles.Add(new Collectible { Category = newCategory });
                _context.SaveChanges();
            }
            return RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(string category)
        {
            var collectiblesWithCategory = _context.Collectibles.Where(c => c.Category == category).ToList();
            _context.Collectibles.RemoveRange(collectiblesWithCategory);
            _context.SaveChanges();
            return RedirectToAction("ManageCategories");
        }

        public IActionResult ManageBrands()
        {
            var brands = _context.Collectibles.Select(c => c.Brand).Distinct().ToList();
            return View(brands);
        }

        [HttpPost]
        public IActionResult AddBrand(string newBrand)
        {
            if (!string.IsNullOrWhiteSpace(newBrand) && !_context.Collectibles.Any(c => c.Brand == newBrand))
            {
                _context.Collectibles.Add(new Collectible { Brand = newBrand });
                _context.SaveChanges();
            }
            return RedirectToAction("ManageBrand");
        }

        [HttpPost]
        public IActionResult DeleteBrand(string brand)
        {
            var collectiblesWithBrand = _context.Collectibles.Where(c => c.Brand == brand).ToList();
            _context.Collectibles.RemoveRange(collectiblesWithBrand);
            _context.SaveChanges();
            return RedirectToAction("ManageBrand");
        }


    }
}
