using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Models;

namespace RepeaterCouncil.Data;

public partial class ApplicationDbContext : IdentityDbContext<RepeaterCouncil.Models.User>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<AutopatchOption> AutopatchOptions { get; set; }

    public virtual DbSet<BusinessRulesFrequency> BusinessRulesFrequencies { get; set; }

    public virtual DbSet<EmailQueue> EmailQueues { get; set; }

    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<Frequency> Frequencies { get; set; }

    public virtual DbSet<Key> Keys { get; set; }

    public virtual DbSet<Link> Links { get; set; }

    public virtual DbSet<LinkType> LinkTypes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<ProposedCoordinationAnswer> ProposedCoordinationAnswers { get; set; }

    public virtual DbSet<ProposedCoordinationsLog> ProposedCoordinationsLogs { get; set; }

    public virtual DbSet<Repeater> Repeaters { get; set; }

    public virtual DbSet<RepeaterChangeLog> RepeaterChangeLogs { get; set; }

    public virtual DbSet<RepeaterStatus> RepeaterStatuses { get; set; }

    public virtual DbSet<RepeaterType> RepeaterTypes { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestNote> RequestNotes { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<RequestWorkflow> RequestWorkflows { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:RepeaterData3", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AutopatchOption>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<BusinessRulesFrequency>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FrequencyEnd)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("decimal(12, 6)");
            entity.Property(e => e.FrequencyStart).HasColumnType("decimal(12, 6)");
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.SpacingMhz).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.StateAbbreviation)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmailQueue>(entity =>
        {
            entity.ToTable("EmailQueue");

            entity.Property(e => e.Body).HasColumnType("text");
            entity.Property(e => e.FromEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("hiram@repeatercouncil.org");
            entity.Property(e => e.FromName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("Repeater Council (automated)");
            entity.Property(e => e.Sent).HasColumnType("datetime");
            entity.Property(e => e.Subject).IsUnicode(false);
            entity.Property(e => e.TemplateID)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.ToEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ToName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EventLog>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK_ErrorLog");

            entity.ToTable("EventLog");

            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.jsonData).HasDefaultValueSql("(NULL)");
        });

        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.Property(e => e.Band)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Input).HasColumnType("decimal(9, 4)");
            entity.Property(e => e.Output).HasColumnType("decimal(9, 4)");
        });

        modelBuilder.Entity<Key>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Key1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Key");
        });

        modelBuilder.Entity<Link>(entity =>
        {
            entity.Property(e => e.Comment).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.LinkTypeID).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.LinkFromRepeater).WithMany(p => p.LinkLinkFromRepeaters)
                .HasForeignKey(d => d.LinkFromRepeaterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Links_LinkFromRepeaterID");

            entity.HasOne(d => d.LinkToRepeater).WithMany(p => p.LinkLinkToRepeaters)
                .HasForeignKey(d => d.LinkToRepeaterID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Links_LinkToRepeaterID");

            entity.HasOne(d => d.LinkType).WithMany(p => p.Links)
                .HasForeignKey(d => d.LinkTypeID)
                .HasConstraintName("FK_Links_LinkTypeID");
        });

        modelBuilder.Entity<LinkType>(entity =>
        {
            entity.Property(e => e.LinkType1).HasColumnName("LinkType");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Permissi__3214EC273694552F");

            entity.HasOne(d => d.Repeater).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.RepeaterId)
                .HasConstraintName("FK_Permissions_RepeaterId");

            entity.HasOne(d => d.User).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.IdentityUserId)
                .HasConstraintName("FK_Permissions_UserId");
        });

        modelBuilder.Entity<ProposedCoordinationAnswer>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Proposed__3214EC27E392A2E4");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProposedCoordinationsLog>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Proposed__3214EC27717CDC1F");

            entity.ToTable("ProposedCoordinationsLog");

            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReceiveFrequency).HasColumnType("decimal(12, 6)");
            entity.Property(e => e.TransmitFrequency).HasColumnType("decimal(12, 6)");

            entity.HasOne(d => d.AnswerNavigation).WithMany(p => p.ProposedCoordinationsLogs)
                .HasForeignKey(d => d.Answer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProposedCoordinationsLog_Answer");

            entity.HasOne(d => d.RequestedByUser).WithMany(p => p.ProposedCoordinationsLogs)
                .HasForeignKey(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProposedCoordinationsLog_RequestedByUserId");
        });

        modelBuilder.Entity<Repeater>(entity =>
        {
            entity.HasKey(e => e.ID).HasName("PK__Repeater__3214EC2743D1A15A");

            entity.HasIndex(e => e.IdentityUserId, "Index_TrusteeID");

            entity.Property(e => e.AMSL).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.AdditionalInformation).HasColumnType("text");
            entity.Property(e => e.Analog_InputAccess)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Analog_OutputAccess)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Analog_Width).HasColumnType("decimal(6, 3)");
            entity.Property(e => e.AntennaGain).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.AntennaHeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Callsign)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.CoordinatedAntennaHeight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CoordinatorComments).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.DMR_ColorCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.DMR_ID)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DMR_Network)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DSTAR_Module)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.DateConstruction).HasColumnType("datetime");
            entity.Property(e => e.DateCoordinated).HasColumnType("datetime");
            entity.Property(e => e.DateCoordinationSource).HasColumnType("datetime");
            entity.Property(e => e.DateDecoordinated).HasColumnType("datetime");
            entity.Property(e => e.DateUpdated).HasColumnType("datetime");
            entity.Property(e => e.ERP).HasColumnType("decimal(18, 5)");
            entity.Property(e => e.InputFrequency).HasColumnType("decimal(12, 6)");
            entity.Property(e => e.LegacyMetadata).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.LicenseExpired).HasDefaultValue(false);
            entity.Property(e => e.LicenseeSK).HasDefaultValue(false);
            entity.Property(e => e.NXDN_RAN)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OutputFrequency).HasColumnType("decimal(12, 6)");
            entity.Property(e => e.P25_NAC)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.SiteName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Sponsor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.YSF_DSQ)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e._Latitude).HasColumnType("decimal(10, 7)");
            entity.Property(e => e._Longitude).HasColumnType("decimal(10, 7)");

            entity.HasOne(d => d.AutopatchNavigation).WithMany(p => p.Repeaters)
                .HasForeignKey(d => d.Autopatch)
                .HasConstraintName("FK_Repeaters_Autopatch");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Repeaters)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repeaters_Status");

            entity.HasOne(d => d.Trustee).WithMany(p => p.Repeaters)
                .HasForeignKey(d => d.IdentityUserId)
                .HasConstraintName("FK_Repeaters_TrusteeID");

            entity.HasOne(d => d.TypeNavigation).WithMany(p => p.Repeaters)
                .HasForeignKey(d => d.Type)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Repeaters_Type");
        });

        modelBuilder.Entity<RepeaterChangeLog>(entity =>
        {
            entity.Property(e => e.ChangeDateTime).HasColumnType("datetime");
            entity.Property(e => e.ChangeDescription).IsUnicode(false);

            entity.HasOne(d => d.Repeater).WithMany(p => p.RepeaterChangeLogs)
                .HasForeignKey(d => d.RepeaterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepeaterChangeLogs_RepeaterId");

            entity.HasOne(d => d.User).WithMany(p => p.RepeaterChangeLogs)
                .HasForeignKey(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RepeaterChangeLogs_UserId");
        });

        modelBuilder.Entity<RepeaterStatus>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.Status)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RepeaterType>(entity =>
        {
            entity.Property(e => e.ID).ValueGeneratedOnAdd();
            entity.Property(e => e.Type)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.Property(e => e.ClosedOn)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.OutputFrequency).HasColumnType("decimal(12, 6)");
            entity.Property(e => e.RequestedOn)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Repeater).WithMany(p => p.Requests)
                .HasForeignKey(d => d.RepeaterID)
                .HasConstraintName("FK_Requests_RepeaterID");

            entity.HasOne(d => d.User).WithMany(p => p.Requests)
                .HasForeignKey(d => d.IdentityUserId)
                .HasConstraintName("FK_Requests_UserID");
        });

        modelBuilder.Entity<RequestNote>(entity =>
        {
            entity.Property(e => e.Note).IsUnicode(false);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestNotes)
                .HasForeignKey(d => d.RequestID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestNotes_RequestID");

            entity.HasOne(d => d.User).WithMany(p => p.RequestNotes)
                .HasForeignKey(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestNotes_UserID");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RequestWorkflow>(entity =>
        {
            entity.Property(e => e.LastReminderSent)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).IsUnicode(false);
            entity.Property(e => e.RequestedTimeStamp).HasColumnType("datetime");
            entity.Property(e => e.State)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.StatusID).HasDefaultValue(1);
            entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            entity.Property(e => e.UrlKey)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .IsFixedLength();

            entity.HasOne(d => d.Request).WithMany(p => p.RequestWorkflows)
                .HasForeignKey(d => d.RequestID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RequestWorkflows_RequestID");

            entity.HasOne(d => d.Status).WithMany(p => p.RequestWorkflows)
                .HasForeignKey(d => d.StatusID)
                .HasConstraintName("FK_RequestWorkflows_StatusID");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.State1).HasName("PK__States__BA803DACF4C6B60B");

            entity.HasIndex(e => e.State1, "pk_States").IsUnique();

            entity.Property(e => e.State1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("State");
            entity.Property(e => e.CoordinatorEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbreviation)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.website)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.IdentityUserId);

            entity.HasIndex(e => e.ID);

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Callsign)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.LicenseExpired).HasDefaultValue(false);
            entity.Property(e => e.Password)
                .HasMaxLength(8000)
                .HasDefaultValueSql("(NULL)");
            entity.Property(e => e.PhoneCell)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PhoneHome)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PhoneWork)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.SK).HasDefaultValue(false);
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ZIP)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        // Explicitly configure Identity relationships to use IdentityUserId
        modelBuilder.Entity<IdentityUserClaim<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(uc => uc.UserId)
            .HasPrincipalKey(u => u.IdentityUserId); // Use IdentityUserId

        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ul => ul.UserId)
            .HasPrincipalKey(u => u.IdentityUserId); // Use IdentityUserId

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .HasPrincipalKey(u => u.IdentityUserId); // Use IdentityUserId

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
