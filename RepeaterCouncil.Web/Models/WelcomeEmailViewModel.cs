namespace RepeaterCouncil.Web.Models
{
    public class WelcomeEmailViewModel
    {
        public string TenantName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Callsign { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string LoginUrl { get; set; } = string.Empty;
        public DateTime ActivationDate { get; set; } = DateTime.Now;
        public string Subject { get; set; } = string.Empty;
    }
}