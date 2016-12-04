using AnalyseVisitorsTool.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyseVisitorsTool.Data
{
    public class LogDbContext : DbContext
    {
        public DbSet<ServerLogFile> ServerLogFiles { get; set; }
        public DbSet<IPLocation> IPLocations { get; set; }
        public DbSet<Settings> Setup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./AnalyseVisitorsTool.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ServerLogFile>().Property(p => p.ID).UseSqlServerIdentityColumn();            
            builder.Entity<ServerLogFile>().HasKey(k => new { k.ID });
            builder.Entity<IPLocation>().Property(p => p.ID).UseSqlServerIdentityColumn();
            builder.Entity<IPLocation>().HasKey(k => new { k.ID });
            builder.Entity<Settings>().HasKey(k => new { k.ID });
        }
    }
}
