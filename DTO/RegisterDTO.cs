using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO;

public class RegisterDTO
{
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public String Password { get; set; }
    
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public String ConfirmPassword { get; set; }
    
    [Required]
    public String FirstName { get; set; }
    
    [Required]
    public String LastName { get; set; }
    
    [Required]
    public String DisplayName { get; set; }
    
    [Required]
    public int Age { get; set; }
}