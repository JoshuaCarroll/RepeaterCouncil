using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class Permission
{
    public int ID { get; set; }

    public int? UserId { get; set; }

    public int? RepeaterId { get; set; }

    public virtual Repeater? Repeater { get; set; }

    public virtual User? User { get; set; }
    public string IdentityUserId { get; internal set; }
}
