namespace BBS.Core.Models;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Post? Post { get; set; }
}
