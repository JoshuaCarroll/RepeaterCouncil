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
    public class CoordinationRulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoordinationRulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoordinationRules
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CoordinationRules.Include(c => c.Tenant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CoordinationRules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinationRule = await _context.CoordinationRules
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinationRule == null)
            {
                return NotFound();
            }

            return View(coordinationRule);
        }

        // GET: CoordinationRules/Create
        public IActionResult Create()
        {
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name");
            return View();
        }

        // POST: CoordinationRules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenantId,FrequencyStart,FrequencyEnd,SpacingMHz,SeparationMiles")] CoordinationRule coordinationRule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinationRule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });

                // Place a breakpoint here or log to console
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(errors));

                ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", coordinationRule.TenantId);
                return View(coordinationRule);
            }
            
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", coordinationRule.TenantId);
            return View(coordinationRule);
        }

        // GET: CoordinationRules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinationRule = await _context.CoordinationRules.FindAsync(id);
            if (coordinationRule == null)
            {
                return NotFound();
            }
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", coordinationRule.TenantId);
            return View(coordinationRule);
        }

        // POST: CoordinationRules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenantId,FrequencyStart,FrequencyEnd,SpacingMHz,SeparationMiles")] CoordinationRule coordinationRule)
        {
            if (id != coordinationRule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinationRule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinationRuleExists(coordinationRule.Id))
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
            ViewData["TenantId"] = new SelectList(_context.Tenants, "Id", "Name", coordinationRule.TenantId);
            return View(coordinationRule);
        }

        // GET: CoordinationRules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coordinationRule = await _context.CoordinationRules
                .Include(c => c.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinationRule == null)
            {
                return NotFound();
            }

            return View(coordinationRule);
        }

        // POST: CoordinationRules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coordinationRule = await _context.CoordinationRules.FindAsync(id);
            if (coordinationRule != null)
            {
                _context.CoordinationRules.Remove(coordinationRule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinationRuleExists(int id)
        {
            return _context.CoordinationRules.Any(e => e.Id == id);
        }
    }
}
