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
        [HttpPost]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<Order>>>> CreateOrder([FromForm] CreateOrderItemDTO orderItem)
        {
            if (orderItem == null)
            {
                _logger.LogInformation("No data passed");
                return BadRequest(new ResponsiveAPI<string>("Failed", "no data", 500));
            }

            var userData = await _dbContext.Users.FindAsync(orderItem.UserId);
            if (userData == null)
            {
                return BadRequest(new ResponsiveAPI<string>("No user found matching the ID", "Bruh", 404));
            }

            var sizeData = await _dbContext.PaperSizes.FirstOrDefaultAsync(x => x.Id == orderItem.SizeId);
            var frameData = await _dbContext.PaperFrames.FirstOrDefaultAsync(x => x.Id == orderItem.FrameId);

            var initialOrder = new Order
            {
                OrderDate = DateTime.Now,
                Status = "Processing",
                TotalAmount = orderItem.PhotoAmount,    
                ShippingAddress = "92 Xo Viet Nghe Tinh",
                UserId = orderItem.UserId,
                User = userData,
            };

            await _dbContext.Orders.AddAsync(initialOrder);
            await _dbContext.SaveChangesAsync();

            var orderId = initialOrder.Id;

            var submitOrderItem = new OrderItem
            {
                UnitPrice = orderItem.TotalPrice,
                TechnicalSizeId = sizeData.Id,
                TechnicalFrameId = orderItem.FrameId,
                TechnicalTypeName = orderItem.MaterialName,
                TechnicalSize = sizeData,
                TechnicalFrame = frameData,
                OrderId = orderId
            };

            await _dbContext.OrderItems.AddAsync(submitOrderItem);
            await _dbContext.SaveChangesAsync();

            var updateOrder = await _dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId);

            updateOrder.OrderItems.Add(submitOrderItem);
            await _dbContext.SaveChangesAsync();

            return Ok(new ResponsiveAPI<Order>(updateOrder, "Tạo order thành công", 200));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsiveAPI<OrderItem>>>> GetAllOrderItem()
        {
            var result = await _dbContext.OrderItems.ToListAsync();
            return Ok(new ResponsiveAPI<IEnumerable<OrderItem>>(result, "Ok babe", 200));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsiveAPI<OrderItem>>> DeleteOrderItem(int id)
        {
            var delResult = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);
            if (delResult == null)
            {
                return BadRequest(new ResponsiveAPI<string>("Not found", $"OrderItem id:{id} not found", 404));
            }
            _dbContext.OrderItems.Remove(delResult);
            await _dbContext.SaveChangesAsync();
            return Ok(new ResponsiveAPI<OrderItem>(delResult, "Delete ok nha", 200));
        }
        
    }
}
