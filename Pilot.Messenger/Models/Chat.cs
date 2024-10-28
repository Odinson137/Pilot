using System.Collections;
using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Messenger.Interfaces;

namespace Pilot.Messenger.Models;

public class Chat : BaseModel, IAddUser
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Range(1, int.MaxValue)] [Required] public int CreatedBy { get; set; }
    
    public ICollection<ChatMember> ChatMembers { get; set; } = [];

    public ICollection<Message> Messages { get; set; } = [];
    
    public void AddUser(int userId)
    {
        CreatedBy = userId;
    }
}