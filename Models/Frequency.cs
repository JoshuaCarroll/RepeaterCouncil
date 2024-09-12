using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class Frequency
{
    public int ID { get; set; }

    public decimal? Input { get; set; }

    public decimal? Output { get; set; }

    public string? Band { get; set; }
}
