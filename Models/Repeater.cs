using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class Repeater
{
    public int ID { get; set; }

    public byte Type { get; set; }

    public string Callsign { get; set; } = null!;

    public int TrusteeID { get; set; }

    public string IdentityUserId { get; set; }

    public int? _TrusteeOldID { get; set; }

    public byte Status { get; set; }

    public string? City { get; set; }

    public string? SiteName { get; set; }

    public decimal OutputFrequency { get; set; }

    public decimal? InputFrequency { get; set; }

    public string? Sponsor { get; set; }

    public Geometry? Location { get; set; }

    public decimal? _Latitude { get; set; }

    public decimal? _Longitude { get; set; }

    public decimal? AMSL { get; set; }

    public decimal? ERP { get; set; }

    public int? OutputPower { get; set; }

    public decimal? AntennaGain { get; set; }

    public decimal? AntennaHeight { get; set; }

    public string? Analog_InputAccess { get; set; }

    public string? Analog_OutputAccess { get; set; }

    public decimal? Analog_Width { get; set; }

    public string? DSTAR_Module { get; set; }

    public string? DMR_ColorCode { get; set; }

    public string? DMR_ID { get; set; }

    public string? DMR_Network { get; set; }

    public string? P25_NAC { get; set; }

    public string? NXDN_RAN { get; set; }

    public string? YSF_DSQ { get; set; }

    public byte? Autopatch { get; set; }

    public bool? EmergencyPower { get; set; }

    public bool? Linked { get; set; }

    public bool? RACES { get; set; }

    public bool? ARES { get; set; }

    public bool? WideArea { get; set; }

    public bool? Weather { get; set; }

    public bool? Experimental { get; set; }

    public DateTime? DateCoordinated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public DateTime? DateDecoordinated { get; set; }

    public DateTime? DateCoordinationSource { get; set; }

    public DateTime? DateConstruction { get; set; }

    public string? CoordinatorComments { get; set; }

    public string? Notes { get; set; }

    public int? _OldID { get; set; }

    public string? State { get; set; }

    public Geometry? CoordinatedLocation { get; set; }

    public decimal? CoordinatedAntennaHeight { get; set; }

    public int? CoordinatedOutputPower { get; set; }

    public string? AdditionalInformation { get; set; }

    public bool? LicenseeSK { get; set; }

    public bool? LicenseExpired { get; set; }

    public string? LegacyMetadata { get; set; }

    public virtual AutopatchOption? AutopatchNavigation { get; set; }

    public virtual ICollection<Link> LinkLinkFromRepeaters { get; set; } = new List<Link>();

    public virtual ICollection<Link> LinkLinkToRepeaters { get; set; } = new List<Link>();

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<RepeaterChangeLog> RepeaterChangeLogs { get; set; } = new List<RepeaterChangeLog>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual RepeaterStatus StatusNavigation { get; set; } = null!;

    public virtual User? Trustee { get; set; }

    public virtual RepeaterType TypeNavigation { get; set; } = null!;
}
