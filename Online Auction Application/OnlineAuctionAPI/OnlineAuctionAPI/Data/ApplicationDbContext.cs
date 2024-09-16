using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bid>(entity =>
            {
                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Product)
                      .WithMany(p => p.Bids)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                      .WithMany(u => u.Bids)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.StartingPrice)
                      .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReservedPrice)
                      .HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.Products)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { Id = Guid.NewGuid().ToString(),Name = "Admin", Email = "admin@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Admin" },
                    new User { Id = Guid.NewGuid().ToString(),Name = "User 01", Email = "user1@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Normal" },
                    new User { Id = Guid.NewGuid().ToString(),Name = "User 02", Email = "user2@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Normal" },
                    new User {Id = Guid.NewGuid().ToString(),  Name = "User 03", Email = "user3@example.com", PasswordHash =BCrypt.Net.BCrypt.HashPassword("password"), Role = "Normal" },
                    new User { Id = Guid.NewGuid().ToString(),Name = "User 04", Email = "user4@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"), Role = "Normal" },
                    new User {Id = Guid.NewGuid().ToString(),  Name = "User 05", Email = "user5@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password")                
                    , Role = "Normal" }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

        }
    }

}
