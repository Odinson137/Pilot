using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Capability.Models;

[Description("Вакансия")]
public class CompanyPost : BaseModel
{
    [Required] public Post Post { get; set; } = null!;
    
    public ICollection<JobApplication> Applications { get; set; } = [];
    
    [Required] public bool IsOpen { get; set; } = true;
    
    [MaxLength(500)] public string? AdditionalRequirements { get; set; }
    
    [Range(0, 1000000)] public decimal? ExpectedSalary { get; set; }
    
    [Range(0, 50)] public int? RequiredExperienceYears { get; set; }
}