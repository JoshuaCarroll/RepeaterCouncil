using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RepeaterStatus
{
    public byte ID { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Repeater> Repeaters { get; set; } = new List<Repeater>();
}
