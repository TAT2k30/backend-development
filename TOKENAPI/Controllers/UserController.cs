using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TOKENAPI.Models;
using TOKENAPI.Services;
using TOKENAPI.Models;
using BackEndDevelopment.Models.DTOS;

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

        private readonly ILogger<WeatherForecastController> _logger;
        public UserController(DatabaseContext dbContext, ILogger<WeatherForecastController> logger)
        {
            _dbContext = dbContext;

            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<User>>>> GetAllUserAccounts()
        {
            try
            {
                var users = await _dbContext.Users
                    .Include(u => u.Orders)
                    .Include(u => u.Image)
                    .ToListAsync();
                var response = new ResponseiveAPI<IEnumerable<User>>(users, "Data retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseiveAPI<User>.Exception(ex));
            }
        }
        [HttpPost("AddRangeUser")]
       [HttpPost("AddRangeUser")]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<User>>>> CreateRange(List<AddRangeUserDTO> users)
        {
            try
            {
                if (users == null || users.Count == 0)
                {
                    return BadRequest("List of users is empty");
                }

                List<User> usersToAdd = new List<User>();

                foreach (var user in users)
                {
                    var newUser = new User
                    {
                        Email = user.Email,
                        Gender = user.Gender,
                        DateOfBirth = user.DateOfBirth,
                        UserName = user.UserName,
                        Password = user.Password,
                        Role = user.Role
                    };
                    usersToAdd.Add(newUser);
                }

                await _dbContext.Users.AddRangeAsync(usersToAdd);
                await _dbContext.SaveChangesAsync();

                var response = new ResponseiveAPI<IEnumerable<User>>(usersToAdd, "Users created successfully", 201);
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

                var currentDay = DateTime.Now;
                var age = currentDay.Year - user.DateOfBirth.Year;
                _logger.LogInformation("Age :", user.DateOfBirth);
                var rootUrl = "http://localhost:5085/";
                var genderFolder = user.Gender ? "Male" : "Female";
                var baseFolder = $"Uploads/DefaultImage/{genderFolder}";

                if (image == null)
                {
                    _logger.LogInformation("Age setting if image null :");
                    user.AvatarUrl = DetermineDefaultAvatarUrl(age, rootUrl, baseFolder);
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

        private string DetermineDefaultAvatarUrl(int age, string rootUrl, string baseFolder)
        {

            if (age > 5 && age < 10)
            {
                return $"{rootUrl}{baseFolder}/Kid(5-10).jpg";
            }
            else if (age > 10 && age < 20)
            {
                return $"{rootUrl}{baseFolder}/Teenager(10-20).jpg";
            }
            else if (age > 20 && age < 50)
            {
                return $"{rootUrl}{baseFolder}/Adult(20-50).jpg";
            }
            else if (age > 50)
            {
                return $"{rootUrl}{baseFolder}/Senior.jpg";
            }
            else
            {
                return null; 
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
