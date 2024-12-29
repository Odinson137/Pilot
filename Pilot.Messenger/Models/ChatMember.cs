using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Messenger.Models;

public class ChatMember : BaseModel
{
    [Required] public Chat Chat { get; set; } = null!;

    [Required] public int UserId { get; set; }
}