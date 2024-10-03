using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RepeaterType
{
    public byte ID { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Repeater> Repeaters { get; set; } = new List<Repeater>();
}
