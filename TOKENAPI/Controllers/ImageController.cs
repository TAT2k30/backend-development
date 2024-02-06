using BackEndDevelopment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace BackEndDevelopment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public ImageController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<Image>>>> Get()
        {
            var image = await _dbContext.Images.ToListAsync();
            return Ok(new ResponseiveAPI<IEnumerable<Image>>(image, "Get all paper sizes successfully", 200));
        }
        
    }
}
