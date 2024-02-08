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
        public async Task<ActionResult<ResponseiveAPI<Image>>> Create([FromForm] ICollection<Image> images, ICollection<IFormFile> files, int userID)
        {
            if (!ModelState.IsValid || files == null || !files.Any())
            {
                return BadRequest(ResponseiveAPI<Image>.BadRequest(ModelState));
            }
        
            List<string> imgUrl = new List<string>();
            foreach (var file in files)
            {
                var result = FileHandler.SaveImage("PrintingPhoto", file);
                imgUrl.Add(result);
            }

            List<Image> imageAdding = new List<Image>();
            for (int i = 0; i < images.Count; i++)
            {
         
                if (i >= imgUrl.Count)
                {
                    return BadRequest(new ResponseiveAPI<Image>(null, "Not enough image files provided", 400));
                }
                var newImage = new Image
                {
                    Title = images.ElementAt(i).Title,
                    ImageUrl = imgUrl[i],
                    Description = images.ElementAt(i).Description,
                    UserId = userID
                };
                imageAdding.Add(newImage);
            }

            try
            {
                await _dbContext.Images.AddRangeAsync(imageAdding);
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponseiveAPI<List<Image>>(imageAdding, "Images created successfully", 201));
            }
            catch (Exception ex)
            {
              
                foreach (var img in imgUrl)
                {
                    FileHandler.DeleteImage(img);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseiveAPI<Image>(null, "Error occurred while saving images", 500));
            }
        }


    }
}
