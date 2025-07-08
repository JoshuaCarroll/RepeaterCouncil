using RepeaterCouncil.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace RepeaterCouncil.Web.Models
{
    public class Repeater
    {
        public int Id { get; set; }

        [Display(Name = "Tenant")]
        public int TenantId { get; set; }

        public Tenant? Tenant { get; set; } = null;

        public string Callsign { get; set; }

        public RepeaterType Type { get; set; }

        public RepeaterStatus Status { get; set; } // from code table

        public string City { get; set; }

        [Display(Name = "Location Description")]
        public string SiteDescription { get; set; } = "";

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Display(Name = "Altitude (meters)")]
        public double AltitudeMeters { get; set; }

        [Display(Name = "Output Power (watts)")]
        public double OutputPowerWatts { get; set; }

        [Display(Name = "Effective Radiated Power (watts)")]
        public double EffectiveRadiatedPower { get; set; }

        [Display(Name = "Antenna Gain (dBi)")]
        public double AntennaGain { get; set; }

        [Display(Name = "Antenna Height (meters)")]
        public double AntennaHeightMeters { get; set; }

        [Display(Name = "Repeater transmit frequecy")]
        public double TransmitFreq { get; set; }

        [Display(Name = "Repeater receive frequency")]
        public double ReceiveFreq { get; set; }

        [Display(Name = "Input Tone Type")]
        public ToneSquelchType InputToneType { get; set; }

        [Display(Name = "Input Tone Value")]
        public double? InputToneValue { get; set; }

        [Display(Name = "Output Tone Type")]
        public ToneSquelchType OutputToneType { get; set; }

        [Display(Name = "Output Tone Value")]
        public double? OutputToneValue { get; set; }

        [Display(Name = "Analog Bandwidth (kHz)")]
        public string AnalogBandwidth { get; set; }

        [Display(Name = "Date Coordinated")]
        public DateTime DateCoordinated { get; set; }

        [Display(Name = "Date Last Updated")]
        public DateTime? DateUpdated { get; set; }

        [Display(Name = "Date Decoordinated")]
        public DateTime? DateDecoordinated { get; set; } = null;

        public ICollection<RepeaterNote>? Notes { get; set; } = null;

        public ICollection<Link>? Links { get; set; } = null;
    }

}
