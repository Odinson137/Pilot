using Pilot.Contracts.Base;

namespace Pilot.Worker.Models;

public class TeamEmployee : BaseModel
{
    public Team Team { get; set; } = null!;

    public CompanyUser CompanyUser { get; set; } = null!;
}