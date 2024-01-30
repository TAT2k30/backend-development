using static System.Net.Mime.MediaTypeNames;
using TOKENAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackEndDevelopment.Models.OrderProps;

namespace BackEndDevelopment.Models.OrderProps
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string? ImageUrl { get; set; }
        //Cấu hình quan hệ nhiều nhiều với các bản
        public User? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

    }
}
