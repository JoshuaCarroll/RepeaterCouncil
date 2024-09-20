using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RequestWorkflow
{
    public int ID { get; set; }

    public int RequestID { get; set; }

    public string State { get; set; } = null!;

    public string? Note { get; set; }

    public DateTime? TimeStamp { get; set; }

    public string? UrlKey { get; set; }

    public DateTime? RequestedTimeStamp { get; set; }

    public DateTime? LastReminderSent { get; set; }

    public int? StatusID { get; set; }

    public virtual Request Request { get; set; } = null!;

    public virtual RequestStatus? Status { get; set; }
}
