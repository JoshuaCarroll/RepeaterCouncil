using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class LinkType
{
    public int ID { get; set; }

    public string LinkType1 { get; set; } = null!;

    public virtual ICollection<Link> Links { get; set; } = new List<Link>();
}
