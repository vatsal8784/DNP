using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO;

public class LoginDTO
{
    
    [Required]
    [EmailAddress]
    public String Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public String Password { get; set; }

    [Required] public String UserName;
}