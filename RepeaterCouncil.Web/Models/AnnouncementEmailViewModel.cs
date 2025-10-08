namespace RepeaterCouncil.Web.Models
{
    public class AnnouncementEmailViewModel
    {
        public string TenantName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string PreMessage { get; set; } = string.Empty;
        public string MessageBody { get; set; } = string.Empty;
        public string PostMessage { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
        public string ActionText { get; set; } = string.Empty;
        public string SenderInfo { get; set; } = string.Empty;
    }
}