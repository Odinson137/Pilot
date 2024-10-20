using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Models;

public class Message : BaseModel, IAddUser
{
    [MaxLength(1000)] public string? Text { get; set; }

    [Required] public int UserId { get; set; }

    [Required] public Chat Chat { get; set; } = null!;
    
    public void AddUser(int userId)
    {
        UserId = userId;
    }
}