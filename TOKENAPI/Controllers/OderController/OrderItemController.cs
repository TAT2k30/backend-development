using BackEndDevelopment.Controllers.PaperController;
using BackEndDevelopment.Models.DTOS;
using BackEndDevelopment.Models.OrderProps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using TOKENAPI.Models;
using TOKENAPI.Services;


namespace BackEndDevelopment.Controllers.OderController
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderItemController : ControllerBase
    {
        private readonly ILogger<OrderItemController> _logger;
        private readonly DatabaseContext _dbContext;
        public OrderItemController(DatabaseContext dbContext, ILogger<OrderItemController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ResponsiveAPI<Order>>>> CreateOrder([FromForm] CreateOrderItemDTO orderItem)
        {
            if (orderItem == null)
            {
                _logger.LogInformation("No data passes");
                return BadRequest(new ResponsiveAPI<string>("Failed", "no data", 500));
            }
            var userData = await _dbContext.Users.FindAsync(orderItem.UserId);
            if (userData == null)
            {
                return BadRequest(new ResponsiveAPI<string>("No user found match the ID", "Bruh", 404));
            }


            _logger.LogInformation(
                $"Size id : {orderItem.SizeId}\n" +
                $"Frame id : {orderItem.FrameId}\n" +
                $"Material id: {orderItem.MaterialName}\n" +
                $"Total Price : {orderItem.TotalPrice}\n" +
                $"Total photo : {orderItem.PhotoAmount}");

            var sizeData = await _dbContext.PaperSizes.FindAsync(orderItem.SizeId);
            var frameData = await _dbContext.PaperFrames.FindAsync(orderItem.FrameId);
            var submitOrderItem = new OrderItem
            {

                UnitPrice = orderItem.TotalPrice,
                TechnicalSizeId = sizeData.Id,
                TechnicalFrameId = orderItem.FrameId,
                TechnicalTypeName = orderItem.MaterialName,
                TechnicalSize = sizeData,
                TechnicalFrame = frameData,
            };
            await _dbContext.OrderItems.AddAsync(submitOrderItem);
            //Kiểm tra coi trong Order có giỏ hành của user chưa, nếu
            //chưa thì add giỏ hàng và cái order mới làm
            var existingOrderItems = await _dbContext.Orders
            .Where(oi => oi.UserId == orderItem.UserId).ToListAsync();
            if (existingOrderItems == null)
            {
                var oderSubmit = new Order
                {
                    OrderDate = DateTime.Now,
                    Status = "Processing",
                    TotalAmount = orderItem.PhotoAmount,
                    ShippingAddress = "92 Xo Viet Nghe Tinh",
                    CreatedAt = DateTime.Now,
                    UserId = orderItem.UserId,
                    User = userData,
                    OrderItems = (ICollection<OrderItem>)submitOrderItem
                };
                return Ok(new ResponsiveAPI<IEnumerable<Order>>((IEnumerable<Order>)oderSubmit, "Ok em", 200));
            }
            /* await _dbContext.OrderItems.AddAsync(submitOrderItem);*/
            return Ok();

        }
        /* [HttpGet]
         public async Task<ActionResult<IEnumerable<ResponsiveAPI<OrderItem>>>> GetAllOrderItem()
         {

         }*/
    }
}
