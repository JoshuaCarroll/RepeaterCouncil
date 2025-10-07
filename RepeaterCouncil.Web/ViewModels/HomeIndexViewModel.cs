namespace RepeaterCouncil.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public int ActiveRepeaterCount { get; set; }
        public int TotalCoordinationsProcessed { get; set; } // Placeholder for future implementation
        public string AverageCoordinationTime { get; set; } = "N/A"; // Placeholder for future implementation
        public string TenantName { get; set; } = string.Empty;
    }
}