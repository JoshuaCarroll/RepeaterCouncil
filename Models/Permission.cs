using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class Permission
{
    public int ID { get; set; }

    public int? UserId { get; set; }

    public int? RepeaterId { get; set; }
}
