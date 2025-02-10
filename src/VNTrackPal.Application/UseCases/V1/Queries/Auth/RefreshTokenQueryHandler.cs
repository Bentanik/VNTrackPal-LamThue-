using VNTrackPal.Contract.DTOs.AuthDTOs;
using VNTrackPal.Contract.Services.Auth;

namespace VNTrackPal.Application.UseCases.V1.Queries.Auth;

public sealed class RefreshTokenQueryHandler
    (ITokenGeneratorService tokenGeneratorService,
    IUnitOfWork unitOfWork,
    ISender sender)
    : IQueryHandler<Query.RefreshTokenQuery, Success<Contract.Services.Auth.Response.LoginResponse>>
{
    public async Task<Result<Success<Contract.Services.Auth.Response.LoginResponse>>>
        Handle(Query.RefreshTokenQuery query, CancellationToken cancellationToken)
    {
        var userIdClaim = tokenGeneratorService.ValidateAndGetUserIdFromRefreshToken(query.RefreshToken);

        if (userIdClaim == null)
            throw new AuthException.LoginTokenExpiredException();

        var userId = Guid.Parse(userIdClaim);

        var user = await unitOfWork.UserRepository
            .FindSingleAsync(u => u.Id == userId, cancellationToken, u => u.Role);

        if (user == null)
            throw new AuthException.LoginTokenExpiredException();

        // Generate access and refresh token
        var accessToken = tokenGeneratorService.GenerateAccessToken(userId, user.Role.Name);

        var refreshToken = tokenGeneratorService.GenerateRefreshToken(userId, user.Role.Name);

        var authTokenDto = new AuthTokenDTO(accessToken, refreshToken, AuthConstant.BearerTokenScheme);

        var authUserDto = new AuthUserDTO(user.Email, user.FullName, user.PublicAvatarUrl);

        //  Response
        var loginDto = new LoginDTO(authTokenDto, authUserDto);

        var response = new Contract.Services.Auth.Response.LoginResponse(loginDto);

        return Result.Success(new Success<Contract.Services.Auth.Response.LoginResponse>
        (AuthMessage.RefreshTokenSuccessfully.GetMessage().Code,
         AuthMessage.RefreshTokenSuccessfully.GetMessage().Message, response));
    }
}
