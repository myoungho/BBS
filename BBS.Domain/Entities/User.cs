using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBS.Domain.Enums;

namespace BBS.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string LoginId { get; set; } = default!; // Email as login ID

    [Required]
    public string Nickname { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;

    public List<Role> Roles { get; set; } = new() { Role.Reader };
}

