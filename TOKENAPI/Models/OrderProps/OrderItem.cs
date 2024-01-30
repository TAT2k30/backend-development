using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.PaperProperties;
using BackEndDevelopment.Models.ProductProps;
namespace BackEndDevelopment.Models.OrderProps
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? TechnicalSizeId { get; set; }
        public int? TechnicalTypeId { get; set; }
        public int? TechnicalFrameId { get; set; }

        // Navigation Properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public PaperSize? TechnicalSize { get; set; }
        public PaperType? TechnicalType { get; set; }
        public PaperFrame? TechnicalFrame { get; set; }
    }
}
