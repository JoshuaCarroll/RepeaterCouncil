namespace RepeaterCouncil.Web.Models
{
    public class EmailConfirmationViewModel
    {
        public string TenantName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }
}