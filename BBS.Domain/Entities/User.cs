using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBS.Domain.Enums;

namespace BBS.Domain.Entities;

public class User
{
    [Key]
    public string Id { get; set; } = default!; // Email as ID

    [Required]
    public string Nickname { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;

    public List<Role> Roles { get; set; } = new() { Role.Reader };
}

