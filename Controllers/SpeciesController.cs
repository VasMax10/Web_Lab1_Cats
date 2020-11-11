using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Lab1_Cats.Models;

namespace Web_Lab1_Cats.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly CatsContext _context;
        public SpeciesController(CatsContext context)
        {
            _context = context;
        }

        // GET: Species
        public async Task<IActionResult> Index()
        {
            return View(await _context.Species.ToListAsync());
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int? id, string name)
        {

            ViewBag.speciesName = name;
            if (id == null)
            {
                return NotFound();
            }
            var species = await _context.Species
                .FirstOrDefaultAsync(m => m.Id == id);

            List<Cats> cats = new List<Cats>();
            var cas =   from cs in _context.Cats
                        where cs.SpeciesId == species.Id
                        select cs;
            foreach (var au in cas)
            {
                cats.Add(au);
            }
            
            if (species == null)
            {
                return NotFound();
            }
            ViewData["catsName"] = cats;
            return View(species);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Species/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Country,Wool,Size,Lifetime,Photo")] Species species)
        {

            int counter = 0;
            foreach (var a in _context.Species)
            {
                if (a.Name == species.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така порода вже існує");
                return View(species);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Add(species);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(species);
            }
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = await _context.Species.FindAsync(id);
            if (species == null)
            {
                return NotFound();
            }
            return View(species);
        }

        // POST: Species/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Country,Wool,Size,Lifetime,Photo")] Species species)
        {
            if (id != species.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(species);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeciesExists(species.Id))
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
            return View(species);
        }

        // GET: Species/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var species = await _context.Species
                .FirstOrDefaultAsync(m => m.Id == id);
            if (species == null)
            {
                return NotFound();
            }

            return View(species);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var species = await _context.Species.FindAsync(id);
            var cat =   from c in _context.Cats
                        where c.SpeciesId == species.Id
                        select c;
            foreach (var c in cat)
            {
                _context.Cats.Remove(c);
            }
            _context.Species.Remove(species);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeciesExists(int id)
        {
            return _context.Species.Any(e => e.Id == id);
        }

        public ActionResult AutocompleteSearch(string term)
        {
            List<Species> species = new List<Species>();
            
            var spec = species.Where(a => a.Name.Contains(term))
                            .Select(a => new { name = a.Name })
                            .Distinct();
            //foreach (var sp in species)
            //{
            //    species.Add(sp);
            //}
            return Json(species);
        }
    }
}
