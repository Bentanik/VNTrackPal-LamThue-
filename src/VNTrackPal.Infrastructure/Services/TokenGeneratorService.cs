using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VNTrackPal.Infrastructure.Services;

public sealed class TokenGeneratorService
    : ITokenGeneratorService
{
    private readonly AuthSetting _authSetting;

    public TokenGeneratorService(IOptions<AuthSetting> authSetting)
    {
        _authSetting = authSetting.Value;
    }

    public string GenerateToken
        (string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new
            (issuer, audience, claims, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expirationMinutes), credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(Guid userId, string roleName)
    {
        List<Claim> claims = [
            new Claim("UserId", userId.ToString()),
            new Claim(ClaimTypes.Role, roleName)
        ];
        if (_authSetting.AccessSecretToken != null && _authSetting.Issuer != null && _authSetting.Audience != null)
            return GenerateToken
                (_authSetting.AccessSecretToken,
                _authSetting.Issuer,
                _authSetting.Audience,
                _authSetting.AccessTokenExpMinute,
                claims);

        return null!;
    }

    public string GenerateRefreshToken(Guid userId, string roleName)
    {
        List<Claim> claims = [
            new Claim("UserId", userId.ToString()),
            new Claim(ClaimTypes.Role, roleName)
        ];
        if (_authSetting.RefreshSecretToken != null && _authSetting.Issuer != null && _authSetting.Audience != null)
            return GenerateToken
                (_authSetting.RefreshSecretToken,
                _authSetting.Issuer,
                _authSetting.Audience,
                _authSetting.RefreshTokenExpMinute,
                claims);

        return null!;
    }

    public string GenerateForgotPasswordToken(Guid userId)
    {
        List<Claim> claims = [
            new Claim("UserId", userId.ToString()),
        ];
        return GenerateToken
            (_authSetting.ForgotPasswordSecretToken,
            _authSetting.Issuer,
            _authSetting.Audience,
            _authSetting.ForgotPasswordExpMinute,
            claims);
    }

    public string ValidateAndGetUserIdFromRefreshToken(string refreshToken)
    {
        TokenValidationParameters validationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_authSetting.RefreshSecretToken)),
            ValidIssuer = _authSetting.Issuer,
            ValidAudience = _authSetting.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");

            return userIdClaim?.Value!;
        }
        catch (Exception)
        {
            return null!;
        }
    }

    public string ValidateAndGetUserIdForgotPasswordToken(string forgotPasswordToken)
    {
        TokenValidationParameters validationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_authSetting.ForgotPasswordSecretToken)),
            ValidIssuer = _authSetting.Issuer,
            ValidAudience = _authSetting.Audience,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        try
        {
            var principal = tokenHandler.ValidateToken(forgotPasswordToken, validationParameters, out SecurityToken validatedToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");

            return userIdClaim?.Value!;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null!;
        }
    }
}
