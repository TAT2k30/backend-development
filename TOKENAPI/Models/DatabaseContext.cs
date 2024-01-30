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

   
    }
}
