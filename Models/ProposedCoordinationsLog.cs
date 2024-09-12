using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class ProposedCoordinationsLog
{
    public int ID { get; set; }

    public int RequestedByUserId { get; set; }

    public Geometry? Location { get; set; }

    public decimal TransmitFrequency { get; set; }

    public decimal ReceiveFrequency { get; set; }

    public int Answer { get; set; }

    public string? Comment { get; set; }

    public DateTime? DateTime { get; set; }
}
