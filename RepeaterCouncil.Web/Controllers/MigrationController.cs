using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepeaterCouncil.Web.Services;

namespace RepeaterCouncil.Web.Controllers
{
    [Authorize(Roles = "SiteAdministrator")]
    [Route("Admin/{controller=Home}/{action=Index}/{id?}")]
    public class MigrationController : Controller
    {
        private readonly LegacyDataMigrationService _migrationService;
        private readonly ILogger<MigrationController> _logger;

        public MigrationController(
            LegacyDataMigrationService migrationService,
            ILogger<MigrationController> logger)
        {
            _migrationService = migrationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartMigration()
        {
            try
            {
                // Define your tenant mappings based on legacy state abbreviations
                var tenantMappings = new Dictionary<string, string>
                {
                    { "AR", "arkansasrepeatercouncil.org" },
                    { "AL", "alabama.repeatercouncil.org" },
                    { "MS", "mississippi.repeatercouncil.org" },
                    { "LA", "louisiana.repeatercouncil.org" },
                    { "TN", "tennessee.repeatercouncil.org" }
                    // Add more state abbreviation mappings as needed
                };

                var result = await _migrationService.MigrateDataAsync(tenantMappings);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = result.ToString();
                    _logger.LogInformation("Legacy data migration completed successfully");
                }
                else
                {
                    TempData["ErrorMessage"] = $"Migration failed: {result.ErrorMessage}";
                    _logger.LogError("Legacy data migration failed: {ErrorMessage}", result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Migration failed with exception: {ex.Message}";
                _logger.LogError(ex, "Exception during legacy data migration");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ValidateData()
        {
            // Add validation logic here
            // This could check for data consistency, orphaned records, etc.

            var validationResults = new List<string>();

            try
            {
                // Example validations - customize as needed
                validationResults.Add("Data validation completed");

                TempData["ValidationResults"] = string.Join("<br/>", validationResults);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Validation failed: {ex.Message}";
                _logger.LogError(ex, "Exception during data validation");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}