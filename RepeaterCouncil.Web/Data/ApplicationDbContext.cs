using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Models;

namespace RepeaterCouncil.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CoordinationRule> CoordinationRules { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<RepeaterNote> RepeaterNotes { get; set; }
        public DbSet<Repeater> Repeaters { get; set; }
        public DbSet<Tenant> Tenants { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Link>()
                .HasOne(l => l.LinkedRepeater)
                .WithMany()
                .HasForeignKey(l => l.LinkedRepeaterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Repeater>()
                .HasOne(r => r.Trustee)
                .WithMany()
                .HasForeignKey(r => r.TrusteeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure spatial data types
            modelBuilder.Entity<Tenant>()
                .Property(t => t.Borders)
                .HasColumnType("geography");

            modelBuilder.Entity<Repeater>()
                .Property(r => r.Location)
                .HasColumnType("geography");
        }
    }
}
