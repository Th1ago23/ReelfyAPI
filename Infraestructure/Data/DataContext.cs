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
        public DbSet<ContentsList> ContentsLists { get; set; }
        public DbSet<FavoriteContent> FavoriteContents { get; set; }
        public DbSet<AlreadySeenContent> AlreadySeenContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Preference)
                .WithOne(p => p.User)
                .HasForeignKey<Preference>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<ContentsList>()
                .HasOne(ucl => ucl.User)
                .WithMany(u => u.ContentLists)
                .HasForeignKey(ucl => ucl.UserId);

            modelBuilder.Entity<ContentsList>()
                .HasMany(ucl => ucl.Contents)
                .WithMany(c => c.InUserContentLists);

            modelBuilder.Entity<FavoriteContent>()
                .HasKey(fc => fc.Id);

            modelBuilder.Entity<FavoriteContent>()
                .HasOne(fc => fc.User)
                .WithMany(u => u.FavoriteContents)
                .HasForeignKey(fc => fc.UserId);

            modelBuilder.Entity<FavoriteContent>()
                .HasOne(fc => fc.Content)
                .WithMany(c => c.FavoritedByUsers)
                .HasForeignKey(fc => fc.ContentId);

            modelBuilder.Entity<AlreadySeenContent>()
                .HasKey(asc => asc.Id);

            modelBuilder.Entity<AlreadySeenContent>()
                .HasOne(asc => asc.User)
                .WithMany(u => u.AlreadySeenContents)
                .HasForeignKey(asc => asc.UserId);

            modelBuilder.Entity<AlreadySeenContent>()
                .HasOne(asc => asc.Content)
                .WithMany(c => c.SeenInLists)
                .HasForeignKey(asc => asc.ContentId);
        }
    }
}
