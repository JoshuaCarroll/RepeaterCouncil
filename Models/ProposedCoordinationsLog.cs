using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class ProposedCoordinationsLog
{
    public int ID { get; set; }

    public int RequestedByUserID { get; set; }

    public string IdentityUserId { get; set; }

    public Geometry? Location { get; set; }

    public decimal TransmitFrequency { get; set; }

    public decimal ReceiveFrequency { get; set; }

    public int Answer { get; set; }

    public string? Comment { get; set; }

    public DateTime? DateTime { get; set; }

    public virtual ProposedCoordinationAnswer AnswerNavigation { get; set; } = null!;

    public virtual User RequestedByUser { get; set; } = null!;
}
