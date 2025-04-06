using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels.UserViewModels;

public class UserViewModel : BaseViewModel
{
    [Required] [MaxLength(50)] public string UserName { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string Name { get; set; } = null!;
    
    [Required] [MaxLength(50)] public string LastName { get; set; } = null!;
    
    [MaxLength(100)] public string? Email { get; set; }

    [Required] public Role Role { get; init; }

    [MaxLength(1000)] public string? Description { get; set; }
    
    [MaxLength(100)] public string? Country { get; set; }
    
    [MaxLength(100)] public string? City { get; set; }
    
    [File]
    [MaxLength(100)] public string? AvatarImage { get; set; }
    
    public Gender Gender { get; set; }
    
    public DateTime? Birthday { get; set; }

    public string FullName => $"{LastName} {Name} ({UserName})";
}