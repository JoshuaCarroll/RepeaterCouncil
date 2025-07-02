using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.Controllers
{
    public class RepeatersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepeatersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Repeaters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Repeaters.Include(r => r.Tenant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Repeaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeater = await _context.Repeaters
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repeater == null)
            {
                return NotFound();
            }

            return View(repeater);
        }

        // GET: Repeaters/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Set<Tenant>(), "Id", "Id");
            return View();
        }

        // POST: Repeaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,Callsign,Type,Status,City,SiteDescription,Latitude,Longitude,AltitudeMeters,OutputPowerWatts,EffectiveRadiatedPower,AntennaGain,AntennaHeightMeters,TransmitFreq,ReceiveFreq,InputToneType,InputToneValue,OutputToneType,OutputToneValue,AnalogBandwidth,DateCoordinated,DateUpdated,DateDecoordinated")] Repeater repeater)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repeater);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TenantId"] = new SelectList(_context.Set<Tenant>(), "Id", "Id", repeater.TenantId);
            return View(repeater);
        }

        // GET: Repeaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeater = await _context.Repeaters.FindAsync(id);
            if (repeater == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Set<Tenant>(), "Id", "Id", repeater.TenantId);
            return View(repeater);
        }

        // POST: Repeaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,Callsign,Type,Status,City,SiteDescription,Latitude,Longitude,AltitudeMeters,OutputPowerWatts,EffectiveRadiatedPower,AntennaGain,AntennaHeightMeters,TransmitFreq,ReceiveFreq,InputToneType,InputToneValue,OutputToneType,OutputToneValue,AnalogBandwidth,DateCoordinated,DateUpdated,DateDecoordinated")] Repeater repeater)
        {
            if (id != repeater.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repeater);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepeaterExists(repeater.Id))
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
            ViewData["TenantId"] = new SelectList(_context.Set<Tenant>(), "Id", "Id", repeater.TenantId);
            return View(repeater);
        }

        // GET: Repeaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repeater = await _context.Repeaters
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repeater == null)
            {
                return NotFound();
            }

            return View(repeater);
        }

        // POST: Repeaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repeater = await _context.Repeaters.FindAsync(id);
            if (repeater != null)
            {
                _context.Repeaters.Remove(repeater);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepeaterExists(int id)
        {
            return _context.Repeaters.Any(e => e.Id == id);
        }
    }
}
