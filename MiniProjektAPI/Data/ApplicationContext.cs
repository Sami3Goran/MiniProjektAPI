using Microsoft.EntityFrameworkCore;
using MiniProjektAPI.Models;

namespace MiniProjektAPI.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Interest> Interests { get; set; }
        public DbSet<InterestLink> InterestLinks { get; set; }
        public DbSet<Person> Person { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    }
}
