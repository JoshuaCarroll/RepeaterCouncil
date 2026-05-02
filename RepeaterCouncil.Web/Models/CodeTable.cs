using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class CodeTable
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public string Type { get; set; }
        public string Value { get; set; }

        [Display(Name = "Display Text")]
        public string DisplayText { get; set; }
    }

}
