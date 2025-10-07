using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public IActionResult Repeaters(string searchTerm, string searchType = "frequency", double? latitude = null, double? longitude = null, double? searchRadius = 25, int page = 1)
    {
        var tenant = (Tenant)HttpContext.Items["Tenant"]!;
        var tenantName = (string)HttpContext.Items["TenantName"]!;
        
        var viewModel = new RepeaterSearchViewModel
        {
            SearchTerm = searchTerm,
            SearchType = searchType,
            Latitude = latitude,
            Longitude = longitude,
            SearchRadius = searchRadius,
            Page = page,
            TenantName = tenantName
        };

        var query = _context.Repeaters
            .Include(r => r.Trustee)
            .Where(r => r.TenantId == tenant.Id)
            .AsQueryable();

        // Apply search filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            switch (searchType?.ToLower())
            {
                case "frequency":
                    if (double.TryParse(searchTerm, out double freq))
                    {
                        // Search within 0.005 MHz of the target frequency
                        query = query.Where(r => Math.Abs(r.TransmitFreq - freq) <= 0.005);
                    }
                    break;
                case "city":
                    query = query.Where(r => r.City.Contains(searchTerm));
                    break;
                case "callsign":
                    query = query.Where(r => r.Callsign.Contains(searchTerm) || 
                                           (r.Trustee != null && r.Trustee.Callsign.Contains(searchTerm)));
                    break;
                case "coordinates":
                    if (latitude.HasValue && longitude.HasValue && searchRadius.HasValue)
                    {
                        // Simple coordinate-based search (for more precise geographic search, consider using PostGIS or similar)
                        var latRange = searchRadius.Value / 69.0; // Approximate miles to degrees
                        var lonRange = searchRadius.Value / (69.0 * Math.Cos(latitude.Value * Math.PI / 180.0));
                        
                        query = query.Where(r => Math.Abs(r.Latitude - latitude.Value) <= latRange &&
                                               Math.Abs(r.Longitude - longitude.Value) <= lonRange);
                    }
                    break;
            }
        }

        // Get total count for pagination
        viewModel.TotalCount = query.Count();

        // Apply pagination and get results
        viewModel.Results = query
            .OrderBy(r => r.TransmitFreq)
            .Skip((page - 1) * viewModel.PageSize)
            .Take(viewModel.PageSize)
            .ToList();

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
