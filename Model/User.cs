using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Model;

public class User : IdentityUser
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String UserName { get; set; }
    public int Age { get; set; }

    public UserRole Role { get; set; }

    public ICollection<Post> Posts { get; set; }

    public String Password { get; set; }

    public List<string> Validate()
    {
        var errors = new List<string>();
        if (string.IsNullOrEmpty(Email))
        {
            errors.Add("Email is required.");
        }

        if (string.IsNullOrEmpty(Password))
        {
            errors.Add("Password is required.");
        }

        return errors;
    }
}
    
  