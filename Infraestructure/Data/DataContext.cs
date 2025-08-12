using Domain.Models;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Models;

namespace ReelfyAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteMovie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
    
}

