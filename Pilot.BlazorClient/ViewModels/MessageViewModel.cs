using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class MessageViewModel : BaseViewModel
{
    [MaxLength(1000)] public string? Text { get; set; }

    [Required] public int UserId { get; set; }

    [Required] public ChatViewModel Chat { get; set; } = null!;
}