using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.PaperProperties;

using BackEndDevelopment.Models.ProductProps;
using Microsoft.EntityFrameworkCore;
using BackEndDevelopment.Models;
namespace TOKENAPI.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PaperSize> PaperSizes { get; set; }
        public DbSet<PaperType> PaperTypes { get; set; }
        public DbSet<PaperFrame> PaperFrames { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.Orders)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId);
                entity.HasMany(e => e.Orders)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.Id);


            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasMany(e => e.OrderItems)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderId);
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);

            });


        }

    }
}
