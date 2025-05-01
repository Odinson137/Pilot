using System.ComponentModel.DataAnnotations;
using Pilot.BlazorClient.ViewModels.UserViewModels;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class JobApplicationViewModel : BaseViewModel
{
    [Required] public CompanyPostViewModel CompanyPost { get; set; } = null!;

    [Required] public int UserId { get; set; }

    private UserViewModel? _user; 
    public UserViewModel? User
    {
        get => _user;
        set
        {
            UserId = value?.Id ?? 0;
            _user = value;
        }
    }
    
    public string? ResumeFileId { get; set; }

    [Required] public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [MaxLength(500)] public string? Message { get; set; } // Сопроводительное письмо
}