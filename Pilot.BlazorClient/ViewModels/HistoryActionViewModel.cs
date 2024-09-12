using System.ComponentModel.DataAnnotations;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels;

public class HistoryActionViewModel : BaseViewModel
{
    [Required] public CompanyUserViewModel CompanyUserViewModel { get; set; } = null!;

    [Required] public ProjectTaskViewModel ProjectTaskViewModel { get; set; } = null!;

    [Required] [MaxLength(500)] public string LastValue { get; set; } = null!;
    
    [Required] public ActionState ActionState { get; set; }

    public void AddCompanyUser(CompanyUserViewModel companyUserViewModel)
    {
        CompanyUserViewModel = companyUserViewModel;
    }
}