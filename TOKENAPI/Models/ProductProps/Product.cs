using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.ProductProps;


namespace BackEndDevelopment.Models.ProductProps
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? Status { get; set; }

        // Navigation Property
        public ProductCategory? Category { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
