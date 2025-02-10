namespace VNTrackPal.Contract.Services.Auth;

public static class Query
{
    public record LoginQuery(string Email, string Password)
        : IQuery<Success<Response.LoginResponse>>;

    public record RefreshTokenQuery(string RefreshToken)
   : IQuery<Success<Response.LoginResponse>>;
}
