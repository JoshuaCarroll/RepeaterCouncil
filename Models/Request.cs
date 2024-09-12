using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class Request
{
    public int ID { get; set; }

    public int? UserID { get; set; }

    public Geometry? Location { get; set; }

    public int? OutputPower { get; set; }

    public int? Altitude { get; set; }

    public int? AntennaHeight { get; set; }

    public decimal? OutputFrequency { get; set; }

    public byte? StatusID { get; set; }

    public string? State { get; set; }

    public DateTime? RequestedOn { get; set; }

    public int? RepeaterID { get; set; }

    public DateTime? ClosedOn { get; set; }
}
