using DatingApp.API.Entities;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions options) : base(options) { }

        public DataContext()
        {
        }

        public DbSet<Value> Values { get; set; }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(p => new { p.SourceUserId, p.LikedUserId });
            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserLike>()
               .HasOne(s => s.LikedUser)
               .WithMany(l => l.LikedByUsers)
               .HasForeignKey(s => s.LikedUserId)
               .OnDelete(DeleteBehavior.NoAction);
        }

    }
}