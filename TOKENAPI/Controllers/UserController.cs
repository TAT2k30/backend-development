using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TOKENAPI.Models;
using TOKENAPI.Services;
using TOKENAPI.Models;

namespace TOKENAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private bool IsDefaultImage(string imageUrl)
        {
            string fileName = Path.GetFileName(imageUrl);
            List<string> defaultImageNames = new List<string>
            {
                "Kid(5-10).jpg",
                "Teenager(10-20).jpg",
                "Adult(20-50).jpg",
                "Senior.jpg"
            };

            return defaultImageNames.Contains(fileName);
        }

        private readonly DatabaseContext _dbContext;

        public UserController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<User>>>> GetAllUserAccounts()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();
                var response = new ResponseiveAPI<IEnumerable<User>>(users, "Data retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseiveAPI<User>.Exception(ex));
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateUserAccount([FromForm] User user, IFormFile? image)
        {
            try
            {
                if (user == null || user.DateOfBirth == null)
                {
                    return BadRequest(ResponseiveAPI<User>.BadRequest(ModelState));
                }

                var rootUrl = "http://localhost:5085/";
                var userBirthDay = DateTime.Parse(user.DateOfBirth.ToString());
                var currentDay = DateTime.Now;
                var age = currentDay.Year - userBirthDay.Year;

                var genderFolder = user.Gender ? "Male" : "Female";
                var baseFolder = $"Uploads/DefaultImage/{genderFolder}";

                switch (age)
                {
                    case var a when a > 5 && a < 10:
                        user.AvatarUrl = $"{rootUrl}{baseFolder}/Kid(5-10).jpg";
                        break;
                    case var a when a > 10 && a < 20:
                        user.AvatarUrl = $"{rootUrl}{baseFolder}/Teenager(10-20).jpg";
                        break;
                    case var a when a > 20 && a < 50:
                        user.AvatarUrl = $"{rootUrl}{baseFolder}/Adult(20-50).jpg";
                        break;
                    case var a when a > 50:
                        user.AvatarUrl = $"{rootUrl}{baseFolder}/Senior.jpg";
                        break;
                    default:
                        return BadRequest(new ResponseiveAPI<User>(user, "Invalid age", 400));
                }

                user.LastLoginTime = currentDay;

                if (image != null)
                {
                    user.AvatarUrl = FileHandler.SaveImage("UserAvatar", image);
                }

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok(new ResponseiveAPI<User>(user, "User created successfully", 201));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseiveAPI<object>.Exception(ex));
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }
                bool isDefaultImage = IsDefaultImage(user.AvatarUrl);
                if (!isDefaultImage)
                {
                    if (user.AvatarUrl != null)
                    {
                        FileHandler.DeleteImage(user.AvatarUrl);
                    }
                }
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponseiveAPI<User>(user, "User deleted successfully", 200));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
