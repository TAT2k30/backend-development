using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.OrderProps;

namespace BackEndDevelopment.Models.PaperProperties
{
    public class PaperType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool? Status { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Navigation Property
        public List<OrderItem>? OrderItems { get; set; }
    }
}
