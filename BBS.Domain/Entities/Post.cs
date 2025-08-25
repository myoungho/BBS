using System.Collections.Generic;

namespace BBS.Domain.Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public List<Comment> Comments { get; set; } = new();
}
