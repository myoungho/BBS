using System;

namespace BBS.Domain.Entities;

public class Attachment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
    public Post? Post { get; set; }
}

