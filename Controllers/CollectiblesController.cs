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
            // Haal de collectibles op uit de database
            var collectibles = await _context.Collectibles
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .ToListAsync();

            // Controleer of de lijst leeg of null is
            if (!collectibles.Any())
            {
                // Voeg een bericht toe voor de gebruiker
                ViewBag.Message = "Er zijn geen collectibles beschikbaar.";
            }

            // Retourneer de lijst aan de view
            return View(collectibles);
        }

        // GET: Collectibles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectible = await _context.Collectibles
                .Include(c => c.Category) // Voeg Category toe
                .Include(c => c.Brand) // Voeg Brand toe
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
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View();
        }

        // POST: Collectibles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Value,Percentage,Rating,CategoryId,BrandId")] Collectible collectible)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); // Logt de validatiefouten
            }
            if (ModelState.IsValid)
            {
                _context.Add(collectible);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
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
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Brands = _context.Brands.ToList();
            return View(collectible);
        }

        // POST: Collectibles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Value,Percentage,Rating,CategoryId,BrandId")] Collectible collectible)
        {
            if (id != collectible.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
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
            // Herlaad dropdowns als er een fout is
            ViewBag.Categories = _context.Categories.ToList() ?? new List<Category>();
            ViewBag.Brands = _context.Brands.ToList() ?? new List<Brand>();
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
                .Include(c => c.Category) // Voeg Category toe
                .Include(c => c.Brand) // Voeg Brand toe
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
            var collectibles = _context.Collectibles
                                        .Include(c => c.Category)
                                        .Include(c => c.Brand)
                                        .AsQueryable();

            // Zoekfilter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                collectibles = collectibles.Where(c => c.Name.Contains(searchQuery) || c.Description.Contains(searchQuery));
            }

            // Categorie filter
            if (categories != null && categories.Any())
            {
                collectibles = collectibles.Where(c => categories.Contains(c.Category.Name));
            }

            // Merk filter
            if (brands != null && brands.Any())
            {
                collectibles = collectibles.Where(c => brands.Contains(c.Brand.Name));
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

            //Bereken het verschil tussen prijs en waarde

            // Categorieën en merken doorgeven aan de View
            ViewBag.Categories = _context.Categories
                                       .Select(c => c.Name)
                                       .ToList();
            foreach (var category in ViewBag.Categories as List<string>)
            {
                Console.WriteLine(category);
            }

            ViewBag.Brands = await _context.Brands
                                           .Select(b => b.Name)
                                           .Distinct()
                                           .ToListAsync();

            // Bereken het verschil per categorie
            var categoryDifferences = _context.Collectibles
                .GroupBy(c => c.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalDifference = g.Sum(c => c.Value - c.Price),
                    AverageDifference = g.Average(c => c.Value - c.Price)
                }).ToList();

            ViewBag.CategoryDifferences = categoryDifferences;

            return View(filteredCollectibles);
        }

        // Overzicht van categorieën
        public IActionResult ManageCategories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult AddCategory(string name)
        {
            if (!string.IsNullOrEmpty(name) && !_context.Categories.Any(c => c.Name == name))
            {
                _context.Categories.Add(new Category { Name = name });
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageCategories));
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageCategories));
        }

        // Overzicht van merken
        public IActionResult ManageBrands()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }

        [HttpPost]
        public IActionResult AddBrand(string name)
        {
            if (!string.IsNullOrEmpty(name) && !_context.Brands.Any(b => b.Name == name))
            {
                _context.Brands.Add(new Brand { Name = name });
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageBrands)); // Of terug naar de huidige pagina
        }

        [HttpPost]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _context.Brands.Find(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(ManageBrands));
        }


    }
}
