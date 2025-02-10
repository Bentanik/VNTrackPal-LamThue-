namespace VNTrackPal.Contract.Infrastructure.Services;

public interface IPasswordHashService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHashed);
}

