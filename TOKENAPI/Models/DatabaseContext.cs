using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.PaperProperties;

using BackEndDevelopment.Models.ProductProps;
using Microsoft.EntityFrameworkCore;
using BackEndDevelopment.Models;
using BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS;

namespace TOKENAPI.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<PaperSize> PaperSizes { get; set; }
        public DbSet<PaperType> PaperTypes { get; set; }
        public DbSet<PaperFrame> PaperFrames { get; set; }
        public DbSet<ProductCategoryImage> ProductCategoryImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Order>()
                 .HasOne(o => o.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .IsRequired(false);

          

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.TechnicalSize)
                .WithMany(ps => ps.OrderItems)
                .HasForeignKey(oi => oi.TechnicalSizeId);

        

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.TechnicalFrame)
                .WithMany(pf => pf.OrderItems)
                .HasForeignKey(oi => oi.TechnicalFrameId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.Category)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(pc => pc.ProductCategoryImages)
                .WithOne(pci => pci.ProductCategory)
                .HasForeignKey(pci => pci.ProductCategoryId);

            modelBuilder.Entity<Image>()
                .HasMany(i => i.ProductCategoryImages)
                .WithOne(pci => pci.Image)
                .HasForeignKey(pci => pci.ImageId);

            modelBuilder.Entity<ProductCategoryImage>()
                .HasKey(pci => new { pci.ProductCategoryId, pci.ImageId });

            modelBuilder.Entity<ProductCategoryImage>()
                .HasOne(pci => pci.ProductCategory)
                .WithMany(pc => pc.ProductCategoryImages)
                .HasForeignKey(pci => pci.ProductCategoryId);

            modelBuilder.Entity<ProductCategoryImage>()
                .HasOne(pci => pci.Image)
                .WithMany(i => i.ProductCategoryImages)
                .HasForeignKey(pci => pci.ImageId);

            modelBuilder.Entity<ProductCategoryImage>()
                .ToTable("ProductCategoriesImages");

            modelBuilder.Entity<Image>()
                .HasOne(i => i.User)
                .WithMany(u => u.Image)
                .HasForeignKey(i => i.UserId);
        }

    }
}
