using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Enums
{
    public enum ToneSquelchType
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Continuous Tone-Coded Squelch System (CTCSS)")]
        CTCSS = 1,
        [Display(Name = "Digital Coded Squelch (DCS)")]
        DCS = 2,
        [Display(Name = "PL Tone")]
        PL = 3
    }
}
