using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class Request
{
    public int ID { get; set; }

    public int UserID { get; set; }

    public string IdentityUserId { get; set; }

    public Geometry? Location { get; set; }

    public int? OutputPower { get; set; }

    public int? Altitude { get; set; }

    public int? AntennaHeight { get; set; }

    public decimal? OutputFrequency { get; set; }

    public string? State { get; set; }

    public DateTime? RequestedOn { get; set; }

    public int? RepeaterID { get; set; }

    public DateTime? ClosedOn { get; set; }

    public int? StatusID { get; set; }

    public virtual Repeater? Repeater { get; set; }

    public virtual ICollection<RequestNote> RequestNotes { get; set; } = new List<RequestNote>();

    public virtual ICollection<RequestWorkflow> RequestWorkflows { get; set; } = new List<RequestWorkflow>();

    public virtual User? User { get; set; }
}
