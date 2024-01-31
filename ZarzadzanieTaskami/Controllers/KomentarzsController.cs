using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieTaskami.Data;
using ZarzadzanieTaskami.Models;

namespace ZarzadzanieTaskami.Controllers
{
    public class KomentarzsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KomentarzsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Komentarzs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Komentarz.Include(k => k.Task);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Komentarzs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentarz = await _context.Komentarz
                .Include(k => k.Task)
                .FirstOrDefaultAsync(m => m.KomentarzId == id);
            if (komentarz == null)
            {
                return NotFound();
            }

            return View(komentarz);
        }

        // GET: Komentarzs/Create
        public IActionResult Create()
        {
            ViewData["TaskId"] = new SelectList(_context.ProjectTask, "TaskId", "Opis");
            return View();
        }

        // POST: Komentarzs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KomentarzId,Tresc,TaskId")] Komentarz komentarz)
        {
            if (ModelState.IsValid)
            {
                _context.Add(komentarz);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaskId"] = new SelectList(_context.ProjectTask, "TaskId", "Opis", komentarz.TaskId);
            return View(komentarz);
        }

        // GET: Komentarzs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentarz = await _context.Komentarz.FindAsync(id);
            if (komentarz == null)
            {
                return NotFound();
            }
            ViewData["TaskId"] = new SelectList(_context.ProjectTask, "TaskId", "Opis", komentarz.TaskId);
            return View(komentarz);
        }

        // POST: Komentarzs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KomentarzId,Tresc,TaskId")] Komentarz komentarz)
        {
            if (id != komentarz.KomentarzId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(komentarz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarzExists(komentarz.KomentarzId))
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
            ViewData["TaskId"] = new SelectList(_context.ProjectTask, "TaskId", "Opis", komentarz.TaskId);
            return View(komentarz);
        }

        // GET: Komentarzs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentarz = await _context.Komentarz
                .Include(k => k.Task)
                .FirstOrDefaultAsync(m => m.KomentarzId == id);
            if (komentarz == null)
            {
                return NotFound();
            }

            return View(komentarz);
        }

        // POST: Komentarzs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komentarz = await _context.Komentarz.FindAsync(id);
            if (komentarz != null)
            {
                _context.Komentarz.Remove(komentarz);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarzExists(int id)
        {
            return _context.Komentarz.Any(e => e.KomentarzId == id);
        }
    }
}
