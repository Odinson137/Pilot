using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Base;

namespace Pilot.Receiver.Models;

public class CompanyUser : BaseModel
{
    [Required] public Company Company { get; set; } = null!;

    public List<Team> Team { get; set; } = [];

    public List<ProjectTask> ProjectTasks { get; set; } = [];
    
    public CompanyRole? CompanyRole { get; set; }
}