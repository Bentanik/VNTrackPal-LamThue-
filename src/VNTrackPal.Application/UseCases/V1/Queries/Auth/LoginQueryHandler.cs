using VNTrackPal.Contract.DTOs.AuthDTOs;
using VNTrackPal.Contract.Services.Auth;

namespace VNTrackPal.Application.UseCases.V1.Queries.Auth;

public sealed class LoginQueryHandler
    (IUnitOfWork unitOfWork, IPasswordHashService passwordHashService, ITokenGeneratorService tokenGenenratorService)
    : IQueryHandler<Query.LoginQuery, Success<Contract.Services.Auth.Response.LoginResponse>>
{
    public async Task<Result<Success<Contract.Services.Auth.Response.LoginResponse>>> Handle(Query.LoginQuery query, CancellationToken cancellationToken)
    {
        // Get user by email
        var user = await unitOfWork.UserRepository
            .FindSingleAsync(predicate: u => u.Email == query.Email,
                            cancellationToken: cancellationToken,
                            includeProperties: u => u.Role);

        // If user not exist
        if (user == null)
            throw new AuthException.UserNotExistException();

        // Check password
        var isCheckPassword = passwordHashService.VerifyPassword(query.Password, user.Password);

        // If password not match
        if (isCheckPassword == false)
            throw new AuthException.PasswordNotMatchException();

        // Genenrate token
        var accessToken = tokenGenenratorService.GenerateAccessToken(user.Id, user.Role.Name);
        var refreshToken = tokenGenenratorService.GenerateRefreshToken(user.Id, user.Role.Name);

        var authTokenDto = new AuthTokenDTO(accessToken, refreshToken, AuthConstant.BearerTokenScheme);

        var authUserDto = new AuthUserDTO(user.Email, user.FullName, user.PublicAvatarUrl);

        //  Contract.Service.Auth.Response
        var loginDto = new LoginDTO(authTokenDto, authUserDto);

        var response = new Contract.Services.Auth.Response.LoginResponse(loginDto);

        return Result.Success(new Success<Contract.Services.Auth.Response.LoginResponse>
        (AuthMessage.LoginSuccessfully.GetMessage().Code,
         AuthMessage.LoginSuccessfully.GetMessage().Message, response));
    }
}
