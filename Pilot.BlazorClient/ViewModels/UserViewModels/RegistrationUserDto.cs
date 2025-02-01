namespace Pilot.BlazorClient.ViewModels.UserViewModels;

public class RegistrationUserViewModel
{
    public string UserName { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    
    public bool AgreeToTerms { get; set; }
}