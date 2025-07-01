using Microsoft.EntityFrameworkCore;

namespace RepeaterCouncil.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // example DbSet
        public DbSet<Repeater> Repeaters { get; set; }
    }
}
