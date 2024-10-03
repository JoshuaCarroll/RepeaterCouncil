using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RepeaterCouncil.Models;

public partial class User : IdentityUser
{
    public int ID { get; set; }

    public string IdentityUserId { get; set; } = Guid.NewGuid().ToString();

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

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<ProposedCoordinationsLog> ProposedCoordinationsLogs { get; set; } = new List<ProposedCoordinationsLog>();

    public virtual ICollection<RepeaterChangeLog> RepeaterChangeLogs { get; set; } = new List<RepeaterChangeLog>();

    public virtual ICollection<Repeater> Repeaters { get; set; } = new List<Repeater>();

    public virtual ICollection<RequestNote> RequestNotes { get; set; } = new List<RequestNote>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
