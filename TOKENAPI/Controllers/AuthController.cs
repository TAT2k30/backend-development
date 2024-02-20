using BackEndDevelopment.Models.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TOKENAPI.Models;
using TOKENAPI.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TOKENAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserController _userController;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _dbContext;
        public AuthController(IConfiguration configuration, DatabaseContext dbContext, ILogger<WeatherForecastController> logger, UserController userController)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
            _userController = userController;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentials credentials) {
            var user = await Authenticate(credentials);
            if (user != null) {

                user.LastLoginTime = DateTime.Now;
                user.Status = true;
                await _dbContext.SaveChangesAsync();
                var tokenString = TokenService.GenerateJSONWebToken(_configuration, user);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return NotFound(new ResponsiveAPI<User>(null, "No account match your credential", 404));
            }
        }
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] RegisterForm registerForm)
        {
            try
            {
                var checkEmail = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == registerForm.Email);
                if (checkEmail != null)
                {
                    return Conflict(new ResponsiveAPI<object>(null, "Email already taken", 409));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponsiveAPI<object>(null, "Invalid input data", 400));
                }

                var submitUser = new User
                {
                
                    UserName = registerForm.UserName,
                    Email = registerForm.Email,
                    Password = registerForm.Password,
                    DateOfBirth = registerForm.DateOfBirth,
                    Gender = registerForm.Gender,
                    Role = "User",
                    Status = registerForm.Status.HasValue ? registerForm.Status.Value : false,
                    LastLoginTime = null,
                    AvatarUrl = null,

                };


                var response = await _userController.CreateUserAccount(submitUser, null);

                if (response is OkObjectResult)
                {
                    return Ok(new ResponsiveAPI<object>(null, "User registered successfully", 200));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponsiveAPI<object>(null, "User registration failed", 500));
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponsiveAPI<object>(null, "Database error during user registration", 500));
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                user.Status = false;
                user.LastLoginTime = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        private async Task<User> Authenticate(UserCredentials userCredentials)
        {
            var currentUser = await _dbContext.Users
                .FirstOrDefaultAsync(
                u => u.Email.ToLower() == userCredentials.Email
                && u.Password == userCredentials.Password
                );
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
