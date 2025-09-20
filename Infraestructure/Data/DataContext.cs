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

            // 1:1 User → Preference sem cascade delete automático
            modelBuilder.Entity<User>()
                .HasOne(u => u.Preference)
                .WithOne(p => p.User)
                .HasForeignKey<Preference>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // evita múltiplos caminhos de cascade

            // N:N automático (EF Core 5+ cria tabelas de junção)
            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Casts)
                .WithMany(c => c.Preferences);

            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Crews)
                .WithMany(c => c.Preferences);

            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Genres)
                .WithMany(g => g.Preferences);

            modelBuilder.Entity<Preference>()
                .HasMany(p => p.Streamings)
                .WithMany(s => s.Preferences);
        }
    }
}
