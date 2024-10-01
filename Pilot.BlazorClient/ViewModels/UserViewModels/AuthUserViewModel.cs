namespace Pilot.BlazorClient.ViewModels.UserViewModels;

public class AuthUserViewModel
{
    public int UserId { get; set; }

    public string Token { get; set; } = null!;
}