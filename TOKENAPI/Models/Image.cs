using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.DTOS_FOR_RELATIONSHIPS;
using BackEndDevelopment.Models.OrderProps;
using BackEndDevelopment.Models.ProductProps;
using TOKENAPI.Models;

namespace BackEndDevelopment.Models
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }   
        public int? ProductCategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        // Navigation Properties
        public User? User { get; set; }
        public ICollection<ProductCategoryImage>? ProductCategoryImages { get; set; }
    }
}
