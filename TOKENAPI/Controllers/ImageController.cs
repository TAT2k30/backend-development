using BackEndDevelopment.Models;
using BackEndDevelopment.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<ImageResponseModel>>>> GetImageByUserId(int userId)
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
                var imageResponseList = images.Select(img => new ImageResponseModel
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl
                }).ToList();

                return Ok(new ResponseiveAPI<IEnumerable<ImageResponseModel>>(imageResponseList, $"Images for user with id : {userId} retrieved successfully", 200));
            }
            else
            {
                return BadRequest(new ResponseiveAPI<string>("No image", "User haven't posted any images", 200));
            }
        }

        public class ImageResponseModel
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseiveAPI<List<Image>>>> Create([FromForm] ICollection<IFormFile> files, [FromForm] string userID, [FromForm] List<string> fileNames)
        {
            List<string> imgUrl = new List<string>();
            int userId = Int32.Parse(userID);
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid || files == null || !files.Any())
                {
                    return BadRequest(new ResponseiveAPI<string>("Not match validation", "Some of the fields do not match your request", 400));
                }


                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new ResponseiveAPI<string>("Fetching User", $"User with id : {userId} not found", 404));
                }

                List<Image> imagesToAdd = new List<Image>();
                int number = 0;
                var fileNum = 0;
                foreach (var file in files)
                {
                    fileNum++;
                }
                _logger.LogInformation($"Files count: {fileNum}/{files.Count}");
                var nameNum = 0;
                foreach(var file in fileNames)
                {
                    nameNum++;
                }
                _logger.LogInformation($"Name List<string> count: {nameNum}/{fileNames.Count}");
                List<string> imgResponse = new List<string>();

                foreach (var file in files)
                {
                    // Lưu file và lấy đường dẫn trả về
                    var result =  FileHandler.SaveImage("PrintingPhoto", file);
                    
                    imgUrl.Add(result);
                    imgResponse.Add(result.ToString());
                var newImage = new Image
                    {
                        Title = !String.IsNullOrEmpty(fileNames[number]) ? fileNames[number] : file.FileName,
                        Description = "",
                        ImageUrl = result,
                        UserId = userId,
                        ProductCategoryId = null,
                    };
                    imagesToAdd.Add(newImage);
                    number++;
                }

              
                await _dbContext.Images.AddRangeAsync(imagesToAdd);
                await _dbContext.SaveChangesAsync();
               
                return Ok(new ResponseiveAPI<List<string>>(imgResponse, "Images created successfully", 201));
            }
            catch (Exception ex)
            {
                // Trong trường hợp có lỗi, xóa tất cả các ảnh đã lưu trữ
                foreach (var img in imgUrl)
                {
                    FileHandler.DeleteImage(img);
                }

                // Ghi log lỗi
                _logger.LogError(ex, "Error creating images");

                // Trả về lỗi 500 và thông báo lỗi
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
