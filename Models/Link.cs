using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class Link
{
    public int ID { get; set; }

    public int LinkFromRepeaterID { get; set; }

    public int LinkToRepeaterID { get; set; }

    public int? LinkTypeID { get; set; }

    public string? Comment { get; set; }
}
