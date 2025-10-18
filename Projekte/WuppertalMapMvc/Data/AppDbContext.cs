using Microsoft.EntityFrameworkCore;
using WuppertalMapMvc.Models;

namespace WuppertalMapMvc.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Stop> Stops { get; set; }
        public DbSet<StopConnection> StopConnections { get; set; }
        public DbSet<Connection> Connections { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
