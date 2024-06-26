﻿using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Identity.Data;

namespace Pilot.Identity.Models;

public class User : BaseUser
{
    [Required] [MaxLength(100)] public required string Password { get; init; }
    [Required] [MaxLength(100)] public Role Role { get; init; } = Role.User;

    // public ICollection<Message>? Messages { get; set; }
    
    public DateTime Timestamp { get; init; } = DateTime.Now;
}