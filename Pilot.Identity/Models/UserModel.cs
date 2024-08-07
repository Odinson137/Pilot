﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Identity.Models;

public class User : BaseUserModel
{
    [Required] [MaxLength(100)] public required string Password { get; set; }
    [Required] [MaxLength(100)] public Role Role { get; init; } = Role.User;

    // public ICollection<Message>? Messages { get; set; }
}