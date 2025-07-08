using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Enums
{
    public enum RepeaterStatus
    {
        Proposed = 1,
        [Display(Name = "Under Construction")]
        UnderConstruction = 2,
        Operational = 3,
        [Display(Name = "Temporarily Offline")]
        TemporarilyOffline = 4,
        [Display(Name = "Suspected Offline")]
        SuspectedOffline = 5,
        [Display(Name = "Decoordinated")]
        Decoordinated = 6,
    }
}
