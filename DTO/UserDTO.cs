namespace WebApplication1.DTO;

public class UserDTO
{
    
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; }
    public String FirstName { get; set; }
    public String LastName { get; set; }
    
    public int Age { get; set; }
}