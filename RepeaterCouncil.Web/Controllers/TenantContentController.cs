using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.Controllers
{
    [Authorize(Roles = "SiteAdministrator,TenantCoordinator")]
    [Route("Admin/{controller=TenantContent}/{action=Index}/{id?}")]
    public class TenantContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TenantContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TenantContent
        public IActionResult Index()
        {
            var tenant = (Tenant)HttpContext.Items["Tenant"]!;
            return View(tenant);
        }

        // GET: TenantContent/EditAboutUs
        public IActionResult EditAboutUs()
        {
            var tenant = (Tenant)HttpContext.Items["Tenant"]!;
            var fullTenant = _context.Tenants.Find(tenant.Id);
            
            if (fullTenant == null)
            {
                return NotFound();
            }

            return View(fullTenant);
        }

        // POST: TenantContent/EditAboutUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(int id, [Bind("Id,Name,Url,AboutUsContent")] Tenant tenant)
        {
            var currentTenant = (Tenant)HttpContext.Items["Tenant"]!;
            
            if (id != currentTenant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTenant = await _context.Tenants.FindAsync(id);
                    if (existingTenant == null)
                    {
                        return NotFound();
                    }

                    // Only update the AboutUsContent, preserve other fields
                    existingTenant.AboutUsContent = tenant.AboutUsContent;
                    
                    _context.Update(existingTenant);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "About Us content updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TenantExists(tenant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(tenant);
        }

        private bool TenantExists(int id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }
    }
}