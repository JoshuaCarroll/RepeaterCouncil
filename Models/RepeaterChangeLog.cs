﻿using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RepeaterChangeLog
{
    public int ID { get; set; }

    public int RepeaterId { get; set; }

    public int UserId { get; set; }

    public DateTime ChangeDateTime { get; set; }

    public string ChangeDescription { get; set; } = null!;
}
