namespace Pilot.BlazorClient.ViewModels;

public class TeamEmployeeViewModel : BaseViewModel
{
    public TeamViewModel Team { get; set; } = null!;

    public CompanyUserViewModel CompanyUser { get; set; } = null!;
}