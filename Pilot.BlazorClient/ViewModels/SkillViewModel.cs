using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class SkillViewModel : BaseViewModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;
    
    public ICollection<PostViewModel> Posts { get; set; } = [];

    public override string ToString()
    {
        return Title;
    }
}