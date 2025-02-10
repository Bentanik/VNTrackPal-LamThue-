namespace VNTrackPal.Contract.Infrastructure.Services;

public interface ITokenGeneratorService
{
    string GenerateToken
        (string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null);
    string GenerateAccessToken(Guid userId, string roleName);
    string GenerateRefreshToken(Guid userId, string roleName);
    string ValidateAndGetUserIdFromRefreshToken(string refreshToken);
    string GenerateForgotPasswordToken(Guid userId);
    string ValidateAndGetUserIdForgotPasswordToken(string forgotPasswordToken);
}
