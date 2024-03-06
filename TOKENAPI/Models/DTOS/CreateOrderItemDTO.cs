using Microsoft.Identity.Client;

namespace BackEndDevelopment.Models.DTOS
{
    public class CreateOrderItemDTO
    {
        public int SizeId { get; set; }
        public string MaterialName { get; set; }
        public int FrameId { get; set; }
        public int PhotoAmount { get; set; }
        public decimal TotalPrice { get; set; } 
        public int UserId { get; set; }
    }
}
