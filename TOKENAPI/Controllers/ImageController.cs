using BackEndDevelopment.Models;
using BackEndDevelopment.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<Image>>>> Get()
        {
            var image = await _dbContext.Images.ToListAsync();
            return Ok(new ResponsiveAPI<IEnumerable<Image>>(image, "Get all images successfully", 200));
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<ImageResponseModel>>>> GetImageByUserId(int userId)
        {
            var userExist = await _dbContext.Users.FindAsync(userId);
            if (userExist == null)
            {
                return BadRequest(new ResponsiveAPI<string>("No user", "No user id match your request", 404));
            }

            var images = await _dbContext.Images
                .Where(i => i.UserId == userId)
                .ToListAsync();

            if (images != null && images.Any())
            {
                var imageResponseList = new List<ImageResponseModel>();

                foreach (var img in images)
                {
                    var base64String = await GetBase64StringFromImageUrl(img.ImageUrl);
                    if (base64String.IsNullOrEmpty())
                    {
                        return BadRequest(new ResponsiveAPI<string>("Failed", "Cannot convert base64 img", 500));
                    }
                    imageResponseList.Add(new ImageResponseModel
                    {
                        Id = img.Id,
                        Base64Image = base64String
                    });
                }

                return Ok(new ResponsiveAPI<IEnumerable<ImageResponseModel>>(imageResponseList, $"Images for user with id : {userId} retrieved successfully", 200));
            }
            else
            {
                return BadRequest(new ResponsiveAPI<string>("No image", "User haven't posted any images", 404));
            }
        }

        private async Task<string> GetBase64StringFromImageUrl(string imageUrl)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    return Convert.ToBase64String(imageBytes);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public class ImageResponseModel
        {
            public int Id { get; set; }
            public string Base64Image { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<ResponsiveAPI<List<Image>>>> Create([FromForm] ICollection<IFormFile> files, [FromForm] string userID, [FromForm] List<string> fileNames)
        {
            List<string> imgUrl = new List<string>();
            int userId = Int32.Parse(userID);
            try
            {
                // Kiểm tra tính hợp lệ của dữ liệu đầu vào
                if (!ModelState.IsValid || files == null || !files.Any())
                {
                    return BadRequest(new ResponsiveAPI<string>("Not match validation", "Some of the fields do not match your request", 400));
                }


                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound(new ResponsiveAPI<string>("Fetching User", $"User with id : {userId} not found", 404));
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
                foreach (var file in fileNames)
                {
                    nameNum++;
                }
                _logger.LogInformation($"Name List<string> count: {nameNum}/{fileNames.Count}");
                List<string> imgResponse = new List<string>();

                foreach (var file in files)
                {
                    // Lưu file và lấy đường dẫn trả về
                    var result = FileHandler.SaveImage("PrintingPhoto", file);

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

                return Ok(new ResponsiveAPI<List<string>>(imgResponse, "Images created successfully", 201));
            }
            catch (Exception ex)
            {
                // Trong trường hợp có lỗi, xóa tất cả các ảnh đã lưu trữ
                foreach (var img in imgUrl)
                {
                    FileHandler.DeleteImage(img);
                }

                _logger.LogError(ex, "Error creating images");


                return StatusCode(StatusCodes.Status500InternalServerError, new ResponsiveAPI<string>(ex.Message, "Error creating images", 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponsiveAPI<Image>>> Delete(int id)
        {
            try
            {
                var imgDel = await _dbContext.Images.FindAsync(id);
                if (imgDel == null)
                {
                    return BadRequest(new ResponsiveAPI<string>("Image not found", $"Image with id : {id} not found.", 404));
                }
                FileHandler.DeleteImage(imgDel.ImageUrl);
                _dbContext.Images.Remove(imgDel);
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponsiveAPI<Image>(imgDel, $"Image with id : {id} deleted successfully.", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponsiveAPI<string>(ex.Message, "Error deleting image", 500));
            }
        }
    }

}
