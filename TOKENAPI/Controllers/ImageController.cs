using BackEndDevelopment.Models;
using BackEndDevelopment.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TOKENAPI.Controllers;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace BackEndDevelopment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly DatabaseContext _dbContext;
        public ImageController(DatabaseContext dbContext, ILogger<ImageController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<Image>>>> Get()
        {
            var image = await _dbContext.Images.ToListAsync();
            return Ok(new ResponseiveAPI<IEnumerable<Image>>(image, "Get all images successfully", 200));
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<string>>>> GetImageByUserId(int userId)
        {
            var userExist = await _dbContext.Users.FindAsync(userId);
            if (userExist == null)
            {
                return BadRequest(new ResponseiveAPI<string>("No user", "No user id match your request", 404));
            }

            var images = await _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync();

            if (images != null && images.Any())
            {
                var imgUrls = images.Select(img => img.ImageUrl).ToList();
                return Ok(new ResponseiveAPI<IEnumerable<string>>(imgUrls, $"Images for user with id : {userId} retrieved successfully", 200));
            }
            else
            {
                return BadRequest(new ResponseiveAPI<string>("No image", "User haven't posted any images", 200));
            }
        }


        [HttpPost]
        public async Task<ActionResult<ResponseiveAPI<List<Image>>>> Create([FromForm] ICollection<IFormFile> files, [FromForm] string userID)
        {
            List<string> imgUrl = new List<string>();
            int userId = Int32.Parse(userID);
            try
            {
                if (!ModelState.IsValid || files == null || !files.Any())
                {
                    return BadRequest(ResponseiveAPI<Image>.BadRequest(ModelState));
                }
                var user = await _dbContext.Users.FindAsync(userId);
                List<Image> imagesToAdd = new List<Image>();
                if (user == null)
                {
                    return NotFound(new ResponseiveAPI<string>("Fetching User", $"User with id : {userId} not found", 404));
                }
                // Lưu đường dẫn được tạo trong Method bên Service vào trong List Url
                foreach (var file in files)
                {
                    var result = FileHandler.SaveImage("PrintingPhoto", file);
                    imgUrl.Add(result);
                    int number = 0;
                    var newImage = new Image
                    {
                        Title = files.ElementAt(number).FileName,
                        Description = "",
                        ImageUrl = imgUrl[number],
                        UserId = userId,
                        ProductCategoryId = null,
                    };
                    imagesToAdd.Add(newImage);
                    await _dbContext.Images.AddAsync(newImage);
                    number++;
                }
                await _dbContext.SaveChangesAsync();
                await _dbContext.Images.ToListAsync();
                return Ok(new ResponseiveAPI<List<Image>>(imagesToAdd, "Images created successfully", 201));
            }
            catch (Exception ex)
            {
                foreach (var img in imgUrl)
                {
                    FileHandler.DeleteImage(img);
                }
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseiveAPI<string>(ex.Message, "Error creating images", 500));
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseiveAPI<Image>>> Delete(int id)
        {
            try
            {
                var imgDel = await _dbContext.Images.FindAsync(id);
                if (imgDel == null)
                {
                    return BadRequest(new ResponseiveAPI<string>("Image not found", $"Image with id : {id} not found.", 404));
                }
                FileHandler.DeleteImage(imgDel.ImageUrl);
                _dbContext.Images.Remove(imgDel);
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponseiveAPI<Image>(imgDel,$"Image with id : {id} deleted successfully.",200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseiveAPI<string>(ex.Message, "Error deleting image", 500));
            }
        }
    }
  
}
