using BackEndDevelopment.Models.DTOS;
using BackEndDevelopment.Models.ProductProps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace BackEndDevelopment.Controllers.ProductController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public ProductController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct()
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)            
                .ToListAsync();
            if (products != null)
            {
                return Ok(new ResponsiveAPI<IEnumerable<Product>>(products, "Data retrieved successfully", 200));
            }
            return BadRequest(new ResponsiveAPI<string>("No data", "There is no product data in database", 404));
        }
        [HttpPost]
        public async Task<ActionResult<IEnumerable<ResponsiveAPI<IEnumerable<Product>>>>> CreateProduct([FromForm]Product product, IFormFile file)
        {
            try
            {
                if (!ModelState.IsValid || file == null)
                {
                    return BadRequest(new ResponsiveAPI<string>("Not match validation", "Some of the fields do not match your request", 400));
                }
                var productFileUrl = FileHandler.SaveImage("ProductPhoto", file);
                var productSubmit = new Product()
                {
                    Name = product.Name,
                    Description = product.Description,
                    PImgUrl = productFileUrl,
                    Status = "Hidden"
                };
                await _dbContext.AddAsync(productSubmit);
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponsiveAPI<Product>(productSubmit, $"Product {productSubmit.Name} created successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponsiveAPI<string>(ex.Message, "Error creating product", 500));
              
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsiveAPI<Product>>> DeleteProduct(int id)
        {
            var productDel = await _dbContext.Products.FindAsync(id);
            if (productDel != null)
            {
                FileHandler.DeleteImage(productDel.PImgUrl);
                 _dbContext.Products.Remove(productDel);
                await _dbContext.SaveChangesAsync();
               return Ok(new ResponsiveAPI<Product>(productDel, $"Product with id :{productDel.Id} deleted successfully", 200));

            }
            return BadRequest(new ResponsiveAPI<string>("Product delete", $"Product with id :{productDel.Id} wasn't delete", 500));

        }
    }
}
