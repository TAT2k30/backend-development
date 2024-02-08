using BackEndDevelopment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            return Ok(new ResponseiveAPI<IEnumerable<Image>>(image, "Get all images successfully", 200));
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<Image>>>> GetImageById(int id)
        {
            var image = await _dbContext.Images
                   .Include(u => u.User)
                   .FirstOrDefaultAsync(u => u.Id == id);
            if (image != null)
            {
                return Ok(new ResponseiveAPI<Image>(image, $"Image with id : {id} retrieved successfully", 200));
            }
            return BadRequest(ResponseiveAPI<Image>.BadRequest(ModelState));
        }
        [HttpPost()]
        public async Task<ActionResult<ResponseiveAPI<Image>>> Create([FromForm] Image image, ICollection<IFormFile> files)
        {
            if (!ModelState.IsValid || files == null || !files.Any())
            {
                return BadRequest(ResponseiveAPI<Image>.BadRequest(ModelState));
            }

        }
        
    }
}
