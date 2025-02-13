using System.ComponentModel.DataAnnotations;

namespace Pilot.BlazorClient.ViewModels;

public class TeamEmployeeViewModel : BaseViewModel
{
    public BaseViewModel Team { get; set; } = null!;

    public BaseViewModel CompanyUser { get; set; } = null!;
}