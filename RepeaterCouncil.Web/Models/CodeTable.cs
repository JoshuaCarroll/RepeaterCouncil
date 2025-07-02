namespace RepeaterCouncil.Web.Models
{
    public class CodeTable
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public string Type { get; set; } // "RepeaterType", "Status", "ToneType", "LinkType", "Option"
        public string Value { get; set; }
        public string DisplayText { get; set; }
    }

}
