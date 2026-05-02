using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepeaterCouncil.Web.Controllers
{
    [Authorize(Roles = "SiteAdministrator,TenantCoordinator")]
    [Route("Admin/{controller=Home}/{action=Index}/{id?}")]
    public class RepeaterNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepeaterNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RepeaterNotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RepeaterNotes.Include(r => r.Repeater).Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RepeaterNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeaterNote = await _context.RepeaterNotes
                .Include(r => r.Repeater)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repeaterNote == null)
            {
                return NotFound();
            }

            return View(repeaterNote);
        }

        // GET: RepeaterNotes/Create
        public IActionResult Create()
        {
            ViewData["RepeaterId"] = new SelectList(_context.Repeaters, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: RepeaterNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RepeaterId,UserId,CreatedAt,Note")] RepeaterNote repeaterNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repeaterNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var repeaters = _context.Repeaters
                .Select(r => new {
                    r.Id,
                    Display = r.Callsign + " - " + r.TransmitFreq + " MHz - " + r.City
                }).ToList();
            ViewData["RepeaterId"] = new SelectList(repeaters, "Id", "Display");

            var users = _context.Users
                .Select(u => new {
                    u.Id,
                    Display = u.Callsign + " - " + u.FullName
                }).ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Display");

            return View(repeaterNote);
        }

        // GET: RepeaterNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeaterNote = await _context.RepeaterNotes.FindAsync(id);
            if (repeaterNote == null)
            {
                return NotFound();
            }

            var repeaters = _context.Repeaters
                .Select(r => new {
                    r.Id,
                    Display = r.Callsign + " - " + r.TransmitFreq + " MHz - " + r.City
                }).OrderBy(r => r.Display).ToList();
            ViewData["RepeaterId"] = new SelectList(repeaters, "Id", "Display");

            var users = _context.Users
                .Select(u => new {
                    u.Id,
                    Display = u.Callsign + " - " + u.FullName
                }).OrderBy(r => r.Display).ToList();
            ViewData["UserId"] = new SelectList(users, "Id", "Display");

            return View(repeaterNote);
        }

        // POST: RepeaterNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RepeaterId,UserId,CreatedAt,Note")] RepeaterNote repeaterNote)
        {
            if (id != repeaterNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repeaterNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepeaterNoteExists(repeaterNote.Id))
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
            ViewData["RepeaterId"] = new SelectList(_context.Repeaters, "Id", "Id", repeaterNote.RepeaterId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", repeaterNote.UserId);
            return View(repeaterNote);
        }

        // GET: RepeaterNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeaterNote = await _context.RepeaterNotes
                .Include(r => r.Repeater)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repeaterNote == null)
            {
                return NotFound();
            }

            return View(repeaterNote);
        }

        // POST: RepeaterNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repeaterNote = await _context.RepeaterNotes.FindAsync(id);
            if (repeaterNote != null)
            {
                _context.RepeaterNotes.Remove(repeaterNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepeaterNoteExists(int id)
        {
            return _context.RepeaterNotes.Any(e => e.Id == id);
        }
    }
}
