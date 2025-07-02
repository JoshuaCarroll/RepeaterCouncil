namespace RepeaterCouncil.Web.Models
{
    public class CoordinationRule
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public double FrequencyStart { get; set; }
        public double FrequencyEnd { get; set; }

        public double SpacingMHz { get; set; }
        public double SeparationMiles { get; set; }
    }

}
