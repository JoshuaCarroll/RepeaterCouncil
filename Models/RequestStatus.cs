using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RequestStatus
{
    public int ID { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<RequestWorkflow> RequestWorkflows { get; set; } = new List<RequestWorkflow>();
}
