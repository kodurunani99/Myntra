using Microsoft.EntityFrameworkCore;
using MyntraAPI.Models;

namespace MyntraAPI.Data
{
    public class MyntraDbContext : DbContext
    {
        public MyntraDbContext(DbContextOptions<MyntraDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Product configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DiscountedPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Category)
                      .WithMany(e => e.Products)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // CartItem configuration
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                      .WithMany(e => e.CartItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.UserId, e.ProductId }).IsUnique();
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderNumber).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.HasOne(e => e.User)
                      .WithMany(e => e.Orders)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.Order)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Product)
                      .WithMany(e => e.OrderItems)
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Men's Clothing", Description = "Fashionable clothing for men", IsActive = true },
                new Category { Id = 2, Name = "Women's Clothing", Description = "Trendy clothing for women", IsActive = true },
                new Category { Id = 3, Name = "Footwear", Description = "Stylish shoes and sandals", IsActive = true },
                new Category { Id = 4, Name = "Accessories", Description = "Fashion accessories and jewelry", IsActive = true }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Men's Casual T-Shirt",
                    Description = "Comfortable cotton t-shirt for casual wear",
                    Price = 29.99m,
                    Brand = "Fashion Brand",
                    Size = "M",
                    Color = "Blue",
                    StockQuantity = 100,
                    CategoryId = 1,
                    IsActive = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Women's Summer Dress",
                    Description = "Beautiful summer dress perfect for any occasion",
                    Price = 59.99m,
                    Brand = "Elegant Style",
                    Size = "S",
                    Color = "Floral",
                    StockQuantity = 50,
                    CategoryId = 2,
                    IsActive = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Running Shoes",
                    Description = "Comfortable running shoes with excellent support",
                    Price = 89.99m,
                    Brand = "SportMax",
                    Size = "10",
                    Color = "Black",
                    StockQuantity = 75,
                    CategoryId = 3,
                    IsActive = true
                }
            );

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@myntra.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 