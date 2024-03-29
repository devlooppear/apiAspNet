// ApplicationDbContext.cs
using apiAspNet.Models;
using Microsoft.EntityFrameworkCore;

namespace apiAspNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<PersonalAccessToken> PersonalAccessTokens { get; set; }
    }
}
