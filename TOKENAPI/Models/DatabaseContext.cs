using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.PaperProperties;

using BackEndDevelopment.Models.ProductProps;
using Microsoft.EntityFrameworkCore;
using BackEndDevelopment.Models;
using BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS;

namespace TOKENAPI.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options):base(options) { }
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
                 .HasOne(o => o.User)
                 .WithMany(u => u.Orders)
                 .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.OrderItem)
                .WithOne(oi => oi.Order)
                .HasForeignKey<OrderItem>(oi => oi.OrderId)
                .IsRequired(false);


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Category)
                .WithMany(c => c.OrderItem)
                .HasForeignKey(oi => oi.CategoryId)
                .IsRequired(false);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.TechnicalSize)
                .WithMany(ps => ps.OrderItems)
                .HasForeignKey(oi => oi.TechnicalSizeId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.TechnicalType)
                .WithMany(pt => pt.OrderItems)
                .HasForeignKey(oi => oi.TechnicalTypeId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.TechnicalFrame)
                .WithMany(pf => pf.OrderItems)
                .HasForeignKey(oi => oi.TechnicalFrameId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.Category)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategoryImage>()
                .HasMany(pci => pci.Category)
                .WithMany(pc => pc.ProductImage)
                .UsingEntity(j => j.ToTable("ProductCategoriesImages"));

            modelBuilder.Entity<ProductCategoryImage>()
                .HasMany(pci => pci.Images)
                .WithMany(i => i.ProductCategoryImages)
                .UsingEntity(j => j.ToTable("ProductCategoriesImages"));

            modelBuilder.Entity<Image>()
                .HasOne(i => i.User)
                .WithMany(u => u.Image)
                .HasForeignKey(i => i.UserId);
        }

    }
}
