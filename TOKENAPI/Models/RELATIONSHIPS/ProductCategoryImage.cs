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
        public int ProductId { get; set; }
        public int ImageId { get; set; }

        //Navigation properties
        public List<ProductCategory>? Category { get; set; }
        public List<Image>? Images { get; set; }
    }
}
