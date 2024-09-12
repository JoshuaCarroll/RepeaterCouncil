using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class EmailQueue
{
    public int ID { get; set; }

    public string? ToName { get; set; }

    public string? ToEmail { get; set; }

    public string? FromName { get; set; }

    public string? FromEmail { get; set; }

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public DateTime? Sent { get; set; }

    public string? TemplateID { get; set; }
}
