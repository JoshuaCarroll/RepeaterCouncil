using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Enums
{
    public enum RepeaterType
    {
        Repeater = 1,
        Link = 2,
        Control = 3,
        Packet = 4,
        Beacon = 5,
        [Display(Name = "Amateur TV")]
        AmateurTv = 6,
        [Display(Name = "Remote Base")]
        RemoteBase = 7,
        [Display(Name = "Closed (Private)")]
        ClosedRepeater = 8,
        None = 9,
        Other = 99
    }
}
