﻿using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RequestNote
{
    public int ID { get; set; }

    public int RequestID { get; set; }

    public int UserID { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Note { get; set; }
}
