using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class ChatMemberViewModel : BaseViewModel
{
    [Required] public ChatViewModel Chat { get; set; } = null!;

    [Required] public int UserId { get; set; }
}