using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDto)
        {
            // Create a new user object
            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Age = registerDto.Age,
                UserName = registerDto.DisplayName,
                Password = registerDto.Password
            };

            // Validate the user object
            var validationResult = user.Validate();
            if (validationResult != null)
            {
                return BadRequest(validationResult);
            }

            // Generate a new JWT token for the user
            var token = GenerateToken(user);

            // Return the JWT token in the response
            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            // Find the user by username and password
            var user = UserList.Users.FirstOrDefault(u =>
                u.UserName == loginDto.UserName && u.Password == loginDto.Password);

            if (user == null)
            {
                return BadRequest("Invalid login credentials");
            }

            // Generate a new JWT token for the user
            var token = GenerateToken(user);

            // Return the JWT token in the response
            return Ok(new { token });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            // Return a list of all users in the system
            var users = UserList.Users;
            var userDtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                UserName = u.UserName
            }).ToList();

            return Ok(userDtos);
        }

        private string GenerateToken(User user)
        {
            // Define the claims to be included in the JWT token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // Define the JWT token parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            // Generate the JWT token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
