using Domain.Models.Contents;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace ReelfyAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Crew> Crews { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Streaming> Streamings { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }



    }

}

