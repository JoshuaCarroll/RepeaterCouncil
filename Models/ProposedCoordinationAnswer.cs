using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class ProposedCoordinationAnswer
{
    public int ID { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<ProposedCoordinationsLog> ProposedCoordinationsLogs { get; set; } = new List<ProposedCoordinationsLog>();
}
