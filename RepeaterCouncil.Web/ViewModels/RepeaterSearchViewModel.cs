using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.ViewModels
{
    public class RepeaterSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? SearchType { get; set; } // "frequency", "city", "callsign", "coordinates"
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? SearchRadius { get; set; } = 25; // miles
        
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        public List<Repeater> Results { get; set; } = new List<Repeater>();
        public string TenantName { get; set; } = string.Empty;
        
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}