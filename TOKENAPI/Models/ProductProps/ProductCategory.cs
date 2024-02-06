using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS;
using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.ProductProps;


namespace BackEndDevelopment.Models.ProductProps
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? ProductId { get; set; }

        // Navigation Property
        public Product? Product { get; set; }
        public ICollection<ProductCategoryImage>? ProductCategoryImages { get; set; } // Add this navigation property
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
