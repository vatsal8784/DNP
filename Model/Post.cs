namespace WebApplication1.Model;

public class Post
{
    public int id { get; set; }
    public String title { get; set; }
    public String body { get; set; }
    public DateTime PostDate { get; set; }
    public String Userid { get; set; }
    public User User { get; set; }
}