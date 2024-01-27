using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOKENAPI.Migrations;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace TOKENAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _dbContext;
        public AuthController(IConfiguration configuration, DatabaseContext dbContext, ILogger<WeatherForecastController> logger)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpPost]
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
                return NotFound(new ResponseiveAPI<User>(null, "No account match your credential", 404));
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
