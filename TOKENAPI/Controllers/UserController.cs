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

        private readonly ILogger<UserController> _logger;
        public UserController(DatabaseContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;

            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<User>>>> GetAllUserAccounts()
        {
            try
            {
                var users = await _dbContext.Users
                    .Include(u => u.Orders)
                    .Include(u => u.Image)
                    .ToListAsync();
                var response = new ResponsiveAPI<IEnumerable<User>>(users, "Data retrieved successfully", 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponsiveAPI<User>.Exception(ex));
            }
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<ResponsiveAPI<User>>> GetUserById(int id)
        {
            try
            {

                var user = await _dbContext.Users
               .Include(u => u.Orders)
               .Include(u => u.Image)
               .FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    return Ok(new ResponsiveAPI<User>(user, "Data retrieved successfully", 200));
                }
                return BadRequest(new ResponsiveAPI<string>("Get data failed", "There is no data match your request", 404));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponsiveAPI<User>.Exception(ex));
            }
        }
       
        [HttpPost("AddRangeUser")]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<User>>>> CreateRange(List<AddRangeUserDTO> users)
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

                var response = new ResponsiveAPI<IEnumerable<User>>(usersToAdd, "Users created successfully", 201);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponsiveAPI<User>.Exception(ex));
            }
        }


        [HttpPost]
        public async Task<ActionResult> CreateUserAccount([FromForm] User user, IFormFile? image)
        {
            try
            {
                if (user == null || user.DateOfBirth == null)
                {
                    return BadRequest(ResponsiveAPI<User>.BadRequest(ModelState));
                }

                var currentDay = DateTime.Now;
                var age = currentDay.Year - user.DateOfBirth.Year;
                if (currentDay.Month < user.DateOfBirth.Month || (currentDay.Month == user.DateOfBirth.Month && currentDay.Day < user.DateOfBirth.Day))
                {
                    age--;
                }

                if (age < 5)
                {
                    return BadRequest("Age must be at least 5 years old to create an account.");
                }

                var rootUrl = "http://localhost:5085/";
                var genderFolder = user.Gender ? "Male" : "Female";
                var baseFolder = $"Uploads/DefaultImage/{genderFolder}";

                if (image == null)
                {
                    user.AvatarUrl = DetermineDefaultAvatarUrl(age, rootUrl, baseFolder);
                }

                user.LastLoginTime = currentDay;

                if (image != null)
                {
                    user.AvatarUrl = FileHandler.SaveImage("UserAvatar", image);
                }

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok(new ResponsiveAPI<User>(user, "User created successfully", 201));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponsiveAPI<object>.Exception(ex));
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
                return Ok(new ResponsiveAPI<User>(user, "User deleted successfully", 200));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
