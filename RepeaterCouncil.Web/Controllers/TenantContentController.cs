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
        public async Task<IActionResult> EditAboutUs(int id, string AboutUsContent)
        {
            var currentTenant = (Tenant)HttpContext.Items["Tenant"]!;

            if (id != currentTenant.Id)
            {
                return NotFound();
            }

            try
            {
                var existingTenant = await _context.Tenants.FindAsync(id);
                if (existingTenant == null)
                {
                    return NotFound();
                }

                // Only update the AboutUsContent, preserve other fields
                existingTenant.AboutUsContent = AboutUsContent;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "About Us content updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log the error or add a breakpoint here for debugging
                TempData["ErrorMessage"] = $"Error saving content: {ex.Message}";

                // Return to edit form with current tenant data
                var tenantForView = await _context.Tenants.FindAsync(id);
                return View(tenantForView);
            }
        }

        private bool TenantExists(int id)
        {
            return _context.Tenants.Any(e => e.Id == id);
        }
    }
}