using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class Company : BaseModel
{
    [Required] [MaxLength(50)] public string Title { get; init; } = null!;

    [MaxLength(500)] public string? Description { get; init; }

    public List<Project> Projects { get; set; } = [];

    public List<CompanyRole> CompanyRoles { get; set; } = [];
    
    public int? LogoId { get; set; }

    public List<int> InsideImages { get; set; } = [];
}