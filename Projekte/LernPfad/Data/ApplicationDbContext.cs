namespace LernPfad.Data
{
    using LernPfad.Models;

    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Lernpfad> Lernpfade { get; set; }
        public DbSet<Lernschritt> Lernschritte { get; set; }
    }

}
