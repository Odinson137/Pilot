using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Worker.Models;

public class CompanyRole : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; init; } = null!;
    
    public List<CompanyUser> CompanyUsers { get; set; } = [];

    public List<Company> Companies { get; set; } = [];
    
    public bool IsBaseRole { get; init; }
}