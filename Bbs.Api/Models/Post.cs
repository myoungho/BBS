namespace Bbs.Api.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<Comment> Comments { get; set; } = new();
}
