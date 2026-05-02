using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Enums;
using RepeaterCouncil.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RepeaterCouncil.Web.Controllers
{
    [Authorize(Roles = "SiteAdministrator,TenantCoordinator")]
    [Route("Admin/{controller=Home}/{action=Index}/{id?}")]
    public class LinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Links
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Links.Include(l => l.Repeater);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .Include(l => l.Repeater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        private void PrepareSelectBoxes()
        {
            var repeaters = _context.Repeaters
                .OrderBy(r => r.Callsign)
                .ThenBy(r => r.TransmitFreq)
                .Select(r => new {
                    r.Id,
                    Display = r.Callsign + " - " + r.TransmitFreq + " MHz"
                }).ToList();

            ViewData["RepeaterId"] = new SelectList(repeaters, "Id", "Display");
            ViewData["LinkedRepeaterId"] = new SelectList(repeaters, "Id", "Display");

            var linkTypes = Enum.GetValues(typeof(LinkType))
                .Cast<LinkType>()
                .Select(lt => new {
                    Id = (int)lt,
                    Name = lt.GetType()
                             .GetField(lt.ToString())
                             ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                             .Cast<DisplayAttribute>()
                             .FirstOrDefault()?.Name ?? lt.ToString()
                }).ToList();
            ViewData["LinkTypes"] = new SelectList(linkTypes, "Id", "Name");
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            PrepareSelectBoxes();
            return View();
        }


        // POST: Links/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RepeaterId,LinkType,LinkDetails,LinkedRepeaterId")] Link link)
        {
            if (ModelState.IsValid)
            {
                _context.Add(link);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PrepareSelectBoxes();
            return View(link);
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }
            PrepareSelectBoxes();
            return View(link);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RepeaterId,LinkType,LinkDetails,LinkedRepeaterId")] Link link)
        {
            if (id != link.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(link);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinkExists(link.Id))
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
            PrepareSelectBoxes();
            return View(link);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var link = await _context.Links
                .Include(l => l.Repeater)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link != null)
            {
                _context.Links.Remove(link);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinkExists(int id)
        {
            return _context.Links.Any(e => e.Id == id);
        }
    }
}
