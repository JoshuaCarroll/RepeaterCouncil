using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class AutopatchOption
{
    public byte ID { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Repeater> Repeaters { get; set; } = new List<Repeater>();
}
