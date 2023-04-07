using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DTO;
using WebApplication1.Model;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace WebApplication1.Services;

public class AuthManager : IAuthManager
{
     private readonly UserManager<User> UserManager;
    private readonly IConfiguration Configuration;

    public AuthManager(UserManager<User> userManager, IConfiguration configuration)
    {
        UserManager = userManager;
        Configuration = configuration;
    }


    public async Task<string> Login(LoginDTO loginDto)
    {
        var user = await UserManager.FindByEmailAsync(loginDto.Email);
        if (user != null && await UserManager.CheckPasswordAsync(user, loginDto.Password))
        {
            var token = GenerateToken(user);
            return token;
        }

        return null;
    }

    public async Task<bool> Register(RegisterDTO registerDto)
    {
        var userExists = await UserManager.FindByNameAsync(registerDto.DisplayName);
        if (userExists != null)
        {
            return false;
        }

        var user = new User
        {
            Email = registerDto.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerDto.DisplayName,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Age = registerDto.Age,
            Password = registerDto.Password
        };

        var result = await UserManager.CreateAsync(user, registerDto.Password);
        return result.Succeeded;
    }

    private String GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName" , user.LastName),
            new Claim("UserId", user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(Configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            Configuration["Jwt:Issuer"],
            Configuration["Jwt:Issuer"],
            claims,
            expires: expires,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}