using BackEndDevelopment.Models.OrderProps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace BackEndDevelopment.Controllers.OderController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly DatabaseContext _dbContext;
        public OrderController(DatabaseContext dbContext, ILogger<OrderController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet] 
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<Order>>>> GetAllOrders()
        {
            var orders = await _dbContext.Orders
                .Include(x => x.OrderItems)
                .ToListAsync();
            if (orders != null)
            {
                return Ok(new ResponsiveAPI<IEnumerable<Order>>(orders, "Ok bae", 200));

            }
            return BadRequest(new ResponsiveAPI<string>("No data", "No data about orders", 404));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<Order>>>> GetById(int id)
        {
            var orderUser = await _dbContext.Orders
                .Where(x => x.UserId == id)
                .Include(x => x.OrderItems)
                .ToListAsync();
            if (orderUser == null)
            {
                return BadRequest(new ResponsiveAPI<string>("Fetch order failed", "Bruh", 404));
            }
            return Ok(new ResponsiveAPI<IEnumerable<Order>>(orderUser, "Ok nha", 200));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsiveAPI<Order>>> DeleteOrder(int id)
        {
            var orderDel = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (orderDel ==null )
            {
                return BadRequest(new ResponsiveAPI<string>("Not found", $"Order id:{id} not found", 404));
            }
            var okok = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.OrderId  == id);
            _dbContext.OrderItems.Remove(okok);
            await _dbContext.SaveChangesAsync();
            _dbContext.Orders.Remove(orderDel);
            await _dbContext.SaveChangesAsync();
            return Ok(new ResponsiveAPI<Order>(orderDel, "Delete ok nha", 200));
        } 
    }
}
