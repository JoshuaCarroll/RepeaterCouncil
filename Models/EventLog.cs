using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class EventLog
{
    public int ID { get; set; }

    public DateTime? TimeStamp { get; set; }

    public string? jsonData { get; set; }

    public string? Type { get; set; }
}
