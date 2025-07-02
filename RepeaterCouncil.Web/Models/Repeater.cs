namespace RepeaterCouncil.Web.Models
{
    public class Repeater
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public string Callsign { get; set; }
        public string Type { get; set; } // from code table
        public string Status { get; set; } // from code table
        public string City { get; set; }
        public string SiteDescription { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AltitudeMeters { get; set; }
        public double OutputPowerWatts { get; set; }
        public double EffectiveRadiatedPower { get; set; }
        public double AntennaGain { get; set; }
        public double AntennaHeightMeters { get; set; }

        public double TransmitFreq { get; set; }
        public double ReceiveFreq { get; set; }

        public string InputToneType { get; set; }
        public double? InputToneValue { get; set; }
        public string OutputToneType { get; set; }
        public double? OutputToneValue { get; set; }
        public string AnalogBandwidth { get; set; }

        public DateTime DateCoordinated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateDecoordinated { get; set; }

        public ICollection<RepeaterNote> Notes { get; set; }
        public ICollection<Link> Links { get; set; }
    }

}
