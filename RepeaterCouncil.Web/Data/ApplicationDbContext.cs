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
    }
}
