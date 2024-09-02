using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class CompanyRole : BaseModel
{
    [Required] [MaxLength(100)] public string Title { get; set; } = null!;

    public List<CompanyUser> CompanyUsers { get; set; } = [];

    [Required] public Company Company { get; set; } = null!;
    
    [Required] public CompanyUser CreatedBy { get; set; } = null!;
}