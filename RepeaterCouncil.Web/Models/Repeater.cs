using RepeaterCouncil.Web.Enums;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepeaterCouncil.Web.Models
{
    public class Repeater
    {
        public int Id { get; set; }

        [Display(Name = "Tenant")]
        public int TenantId { get; set; }

        public Tenant? Tenant { get; set; } = null;

        public required string Callsign { get; set; }

        [Display(Name = "Trustee")]
        public string? TrusteeId { get; set; }

        [Display(Name = "Trustee")]
        public ApplicationUser? Trustee { get; set; }

        public RepeaterType Type { get; set; }

        public RepeaterStatus Status { get; set; } // from code table

        public required string City { get; set; }

        [Display(Name = "Location Description")]
        public string SiteDescription { get; set; } = "";

        [Display(Name = "Location")]
        public Point? Location { get; set; }

        // Helper properties for form binding - not mapped to database
        [NotMapped]
        [Display(Name = "Latitude (decimal degrees)")]
        [Range(-90.0, 90.0, ErrorMessage = "Latitude must be between -90 and 90")]
        public double Latitude
        {
            get => Location?.Y ?? 0.0;
            set
            {
                if (Location == null && (value != 0.0 || Longitude != 0.0))
                {
                    var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
                    Location = geometryFactory.CreatePoint(new Coordinate(Longitude, value));
                }
                else if (Location != null)
                {
                    var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
                    Location = geometryFactory.CreatePoint(new Coordinate(Location.X, value));
                }
            }
        }

        [NotMapped]
        [Display(Name = "Longitude (decimal degrees)")]
        [Range(-180.0, 180.0, ErrorMessage = "Longitude must be between -180 and 180")]
        public double Longitude
        {
            get => Location?.X ?? 0.0;
            set
            {
                if (Location == null && (Latitude != 0.0 || value != 0.0))
                {
                    var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
                    Location = geometryFactory.CreatePoint(new Coordinate(value, Latitude));
                }
                else if (Location != null)
                {
                    var geometryFactory = new GeometryFactory(new PrecisionModel(), 4326);
                    Location = geometryFactory.CreatePoint(new Coordinate(value, Location.Y));
                }
            }
        }

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
        public required string AnalogBandwidth { get; set; }

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
