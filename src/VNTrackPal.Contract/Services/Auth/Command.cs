namespace VNTrackPal.Contract.Services.Auth;


public static class Command
{
    public record RegisterCommand(string Email, string Password, string FullName) : ICommand;
}
