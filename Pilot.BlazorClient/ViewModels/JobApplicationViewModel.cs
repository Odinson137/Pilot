using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class JobApplicationViewModel : BaseViewModel
{
    [Required] public BaseViewModel CompanyPost { get; set; } = null!;

    [Required] public int UserId { get; set; }

    [Required] public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [MaxLength(500)] public string? Message { get; set; } // Сопроводительное письмо
}