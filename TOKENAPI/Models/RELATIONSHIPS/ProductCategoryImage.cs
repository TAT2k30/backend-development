using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.ProductProps;

namespace BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS
{
    public class ProductCategoryImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Navigation properties
        public int? ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; } // Add this navigation property
        public int? ImageId { get; set; }
        public Image? Image { get; set; }
    }
}
