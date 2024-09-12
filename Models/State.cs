using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace RepeaterCouncil.Models;

public partial class State
{
    public string State1 { get; set; } = null!;

    public string? CoordinatorEmail { get; set; }

    public Geometry? Borders { get; set; }

    public string? StateAbbreviation { get; set; }

    public bool? PopulatedInDatabase { get; set; }

    public string? website { get; set; }
}
