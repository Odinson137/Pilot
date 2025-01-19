using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;

namespace Pilot.Capability.Models;

public class JobApplication : BaseModel
{
    [Required] public CompanyPost CompanyPost { get; set; } = null!;

    [Required] public int UserId { get; set; }

    [Required] public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    [MaxLength(500)] public string? Message { get; set; } // Сопроводительное письмо
}