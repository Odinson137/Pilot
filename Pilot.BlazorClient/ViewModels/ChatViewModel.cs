using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class ChatViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    [MaxLength(500)] public string? Description { get; set; }

    [Range(1, int.MaxValue)] [Required] public int CreatedBy { get; set; }
    
    public ICollection<ChatMemberViewModel> ChatMembers { get; set; } = [];
}