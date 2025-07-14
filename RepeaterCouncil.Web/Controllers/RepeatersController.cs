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

        private void PrepareSelectLists()
        {
            ViewData["TenantId"] = new SelectList(_context.Set<Tenant>().OrderBy(t => t.Name), "Id", "Name");

            var repeaterTypes = Enum.GetValues(typeof(RepeaterType))
            .Cast<RepeaterType>()
            .Select(rt => new {
                Id = (int)rt,
                Name = rt.GetType()
                .GetField(rt.ToString())
                ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? rt.ToString()
            }).ToList()
            .OrderBy(rt => rt.Id);
            ViewData["Type"] = new SelectList(repeaterTypes, "Id", "Name");

            var repeaterStatuses = Enum.GetValues(typeof(RepeaterStatus))
            .Cast<RepeaterStatus>()
            .Select(lt => new {
                Id = (int)lt,
                Name = lt.GetType()
                .GetField(lt.ToString())
                ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? lt.ToString()
            }).ToList()
            .OrderBy(rt => rt.Id);
            ViewData["Status"] = new SelectList(repeaterStatuses, "Id", "Name", "Operational");

            var toneSquelchTypes = Enum.GetValues(typeof(ToneSquelchType))
            .Cast<ToneSquelchType>()
            .Select(lt => new {
                Id = (int)lt,
                Name = lt.GetType()
                .GetField(lt.ToString())
                ?.GetCustomAttributes(typeof(DisplayAttribute), false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? lt.ToString()
            }).ToList()
            .OrderBy(rt => rt.Id);
            ViewData["InputToneType"] = new SelectList(toneSquelchTypes, "Id", "Name", "None");
            ViewData["OutputToneType"] = new SelectList(toneSquelchTypes, "Id", "Name", "None");
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
            PrepareSelectLists();

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

            PrepareSelectLists();

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

            PrepareSelectLists();

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
            PrepareSelectLists();
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
