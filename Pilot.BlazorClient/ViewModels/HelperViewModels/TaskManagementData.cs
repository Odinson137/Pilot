namespace Pilot.BlazorClient.ViewModels.HelperViewModels;

public class TaskManagementData
{
    public ICollection<ProjectViewModel> Projects { get; set; } = new List<ProjectViewModel>();
    public ICollection<ProjectTaskViewModel> Tasks { get; set; } = new List<ProjectTaskViewModel>();
}