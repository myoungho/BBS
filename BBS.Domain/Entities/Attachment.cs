namespace BBS.Domain.Entities;

public class Attachment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public Post? Post { get; set; }
}

