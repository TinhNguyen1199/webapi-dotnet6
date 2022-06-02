using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Data;
using MyWebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSettings _appSettings;

        public UserController(MyDbContext context, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }


        [HttpGet("HelloAdmin")]
        [Authorize(Roles = "admin")]
        public IActionResult AdminEndPoint()
        {
            //var data = _context.Users.First();
            var currentUser = GetCurrentUser();
            return Ok($"Hello {currentUser.FullName} and Your role is: {currentUser.Role}"); 
        }

        [HttpGet("HelloUser")]
        [Authorize(Roles = "user")]
        public IActionResult UserEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hello {currentUser.FullName}. This is user page and Your role is: {currentUser.Role}");
        }

        [HttpGet("Public")]
        [Authorize(Roles = "user,admin")]
        public IActionResult UserAndAdminEndPoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hello {currentUser.FullName}. This is Public Page - Your role is: {currentUser.Role}");
        }

        [HttpPost]
        public  ActionResult Validate(LoginModel request)
        {
            var user =  _context.Users.SingleOrDefault(
                 u => u.UserName == request.UserName &&
                  request.Password == u.Password) ;
            if(user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "The Username or Password is Incorrect"
                });
            }
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "authentication successful",
                Data = GenerateToken(user)
            }); ;
        }

        private string GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserName", user.UserName),
                    new Claim("Id", user.Id.ToString()),

                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            return jwtTokenHandler.WriteToken(token);
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    FullName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }
    }
}
