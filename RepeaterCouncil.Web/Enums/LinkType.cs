using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Enums
{
    public enum LinkType
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Radio/RF")]
        RadioRf = 1,
        Allstar = 2,
        Echolink = 3,
        [Display(Name = "D-Star")]
        DStar = 4,
        DMR = 5,
        [Display(Name = "P25")]
        P25 = 6,
        [Display(Name = "NXDN")]
        NXDN = 7,
        [Display(Name = "Yaesu System Fusion")]
        YaesuSystemFusion = 8,
        [Display(Name = "Hamshack Hotline")]
        HamshackHotline = 9,
        [Display(Name = "Hams Over IP")]
        HamsOverIp = 10,
        [Display(Name = "Broadcastify")]
        Broadcastify = 11,
        Other = 99
    }
}
