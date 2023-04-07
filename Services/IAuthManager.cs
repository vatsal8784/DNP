using WebApplication1.DTO;

namespace WebApplication1.Services;

public interface IAuthManager
{
    Task<String> Login(LoginDTO loginDto);
    Task<bool> Register(RegisterDTO registerDto);
}