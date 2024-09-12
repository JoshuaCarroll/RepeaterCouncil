using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class BusinessRulesFrequency
{
    public int ID { get; set; }

    public string StateAbbreviation { get; set; } = null!;

    public decimal FrequencyStart { get; set; }

    public decimal SpacingMhz { get; set; }

    public int SeparationMiles { get; set; }

    public decimal? FrequencyEnd { get; set; }
}
