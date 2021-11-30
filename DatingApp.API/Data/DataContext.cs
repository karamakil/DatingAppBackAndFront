using DatingApp.API.Entities;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Data
{
    public class DataContext : IdentityDbContext<AppUser,AppRole,int,IdentityUserClaim<int>
        ,AppUserRole,IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>
    {

        public DataContext(DbContextOptions options) : base(options) { }

        public DataContext()
        {
        }

        public DbSet<Value> Values { get; set; }

        public DbSet<UserLike> UserLikes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot config = builder.Build();
            optionsBuilder.UseSqlServer("Server=DESKTOP-OJIR502;Database=DatingApp;MultipleActiveResultSets=true;Trusted_Connection=True;Integrated Security=False;User=ES;Password=ES;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasMany(ur => ur.AppUserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(f => f.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(u => u.AppUserRoles)
                .WithOne(t => t.Role)
                .HasForeignKey(f => f.RoleId)
                .IsRequired();

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

            builder.Entity<Message>()
                .HasOne(x => x.Recipient)
                .WithMany(m => m.MessageRecieved)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(x=>x.Sender)
                .WithMany(m=> m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}