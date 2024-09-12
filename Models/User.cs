using System;
using System.Collections.Generic;

namespace RepeaterCouncil.Models;

public partial class User
{
    public int ID { get; set; }

    public int? OldID { get; set; }

    public string Callsign { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZIP { get; set; }

    public string? Email { get; set; }

    public string? PhoneHome { get; set; }

    public string? PhoneWork { get; set; }

    public string? PhoneCell { get; set; }

    public byte[]? Password { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool? SK { get; set; }

    public bool? LicenseExpired { get; set; }
}
