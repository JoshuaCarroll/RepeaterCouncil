using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class CoordinationRule
    {
        public int Id { get; set; }
        public int TenantId { get; set; }

        public Tenant Tenant { get; set; }

        [Display(Name = "Frequency Range Start (MHz)")]
        public double FrequencyStart { get; set; }

        [Display(Name = "Frequency Range End (MHz)")]
        public double FrequencyEnd { get; set; }

        [Display(Name = "Spacing (MHz)")]
        public double SpacingMHz { get; set; }

        [Display(Name = "Separation (miles)")]
        public double SeparationMiles { get; set; }
    }

}
