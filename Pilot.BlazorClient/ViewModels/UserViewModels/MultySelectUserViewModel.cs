using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Data.Enums;

namespace Pilot.BlazorClient.ViewModels.UserViewModels;

public class MultySelectUserViewModel : BaseViewModel
{
    public UserViewModel User { get; set; }
    public bool IsSelected { get; set; }
}