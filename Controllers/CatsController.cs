using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_Lab1_Cats;

namespace Web_Lab1_Cats.Controllers
{
    public class CatsController : Controller
    {
        private readonly CatsContext _context;

        public CatsController(CatsContext context)
        {
            _context = context;
        }

        // GET: Cats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cats.ToListAsync());
        }

        // GET: Cats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var cats = await _context.Cats
                .Include(f => f.Species)
                .FirstOrDefaultAsync(m => m.Id == id);
            List<Species> species = new List<Species>();
            if (cats == null)
            {
                return NotFound();
            }

            return View(cats);
        }

        // GET: Cats/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Photo,SpeciesId,Age,Info")] Cats cats)
        {
            int counter = 0;
            foreach (var a in _context.Cats)
            {
                if (a.Name == cats.Name)
                { counter++; break; }
            }
            if (counter != 0)
            {
                ModelState.AddModelError("Name", "Така тваринка вже існує");
                return View(cats);
            }
            else
            {
                if (ModelState.IsValid)
                {
                _context.Add(cats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                return View(cats);
            }
        }

        // GET: Cats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cats = await _context.Cats.FindAsync(id);
            if (cats == null)
            {
                return NotFound();
            }
            return View(cats);
        }

        // POST: Cats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Photo,SpeciesId,Age,Info")] Cats cats)
        {
            if (id != cats.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cats);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatsExists(cats.Id))
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
            return View(cats);
        }

        // GET: Cats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cats = await _context.Cats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cats == null)
            {
                return NotFound();
            }

            return View(cats);
        }

        // POST: Cats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cats = await _context.Cats.FindAsync(id);
            _context.Cats.Remove(cats);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatsExists(int id)
        {
            return _context.Cats.Any(e => e.Id == id);
        }
    }
}
