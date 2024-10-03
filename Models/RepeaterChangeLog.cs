using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class RepeaterChangeLog
{
    public int ID { get; set; }

    public int RepeaterId { get; set; }

    public int UserID { get; set; }

    public string IdentityUserId { get; set; }

    public DateTime ChangeDateTime { get; set; }

    public string ChangeDescription { get; set; } = null!;

    public virtual Repeater Repeater { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
