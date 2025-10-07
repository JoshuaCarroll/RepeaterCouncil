using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Enums;
using RepeaterCouncil.Web.Models;
using RepeaterCouncil.Web.ViewModels;

namespace RepeaterCouncil.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var tenant = (Tenant)HttpContext.Items["Tenant"]!;
        var tenantName = (string)HttpContext.Items["TenantName"]!;
        
        // Count active repeaters (Operational, Under Construction, Temporarily Offline)
        var activeRepeaterCount = _context.Repeaters
            .Where(r => r.TenantId == tenant.Id && 
                   (r.Status == RepeaterStatus.Operational || 
                    r.Status == RepeaterStatus.UnderConstruction || 
                    r.Status == RepeaterStatus.TemporarilyOffline))
            .Count();

        var viewModel = new HomeIndexViewModel
        {
            ActiveRepeaterCount = activeRepeaterCount,
            TotalCoordinationsProcessed = 0, // Placeholder - will be implemented when coordination request tracking is added
            AverageCoordinationTime = "N/A", // Placeholder - will be implemented when coordination request tracking is added
            TenantName = tenantName
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Procedures()
    {
        return View();
    }

    public IActionResult Repeaters()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
