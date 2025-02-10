using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using VNTrackPal.Contract.Common.Constants;
using VNTrackPal.Contract.Common.Messages;
using VNTrackPal.Contract.Exceptions.BussinessExceptions;
using VNTrackPal.Contract.Services.Auth;
using VNTrackPal.Contract.Settings;

namespace VNTrackPal.Presentation.Controllers.V1;

public class AuthController : ApiController
{
    private readonly AuthSetting _authSetting;
    public AuthController(ISender sender, IOptions<AuthSetting> AuthSetting) : base(sender)
    {
        _authSetting = AuthSetting.Value;
    }

    [HttpPost("register", Name = "Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromBody] Command.RegisterCommand request)
    {
        var result = await Sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Ok(result);
    }

    [HttpPost("login", Name = "LoginQuery")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromBody] Query.LoginQuery Login)
    {
        var result = await Sender.Send(Login);
        if (result.IsFailure)
            return HandlerFailure(result);

        var value = result.Value;

        var refreshTokenExpMinute = _authSetting.RefreshTokenExpMinute;

        Response.Cookies.Append(AuthConstant.RefreshToken,
            value.Data.LoginDto.AuthTokenDTO.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(refreshTokenExpMinute),
            });

        var loginDto = value.Data.LoginDto with
        {
            AuthTokenDTO = value.Data.LoginDto.AuthTokenDTO with
            {
                RefreshToken = null // Remove refresh token when return
            }
        };

        return Ok(loginDto);
    }

    [HttpGet("refresh-token", Name = "RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies[AuthConstant.RefreshToken];
        if (refreshToken == null) throw new AuthException.LoginTokenExpiredException();

        var result = await Sender.Send(new Query.RefreshTokenQuery(refreshToken));
        if (result.IsFailure)
            return HandlerFailure(result);

        var value = result.Value;

        var refreshTokenExpMinute = _authSetting.RefreshTokenExpMinute;

        Response.Cookies.Append(AuthConstant.RefreshToken,
            value.Data.LoginDto.AuthTokenDTO.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(refreshTokenExpMinute),
            });

        var loginDto = value.Data.LoginDto with
        {
            AuthTokenDTO = value.Data.LoginDto.AuthTokenDTO with
            {
                RefreshToken = null // Remove refresh token when return
            }
        };

        return Ok(loginDto);
    }

    [Authorize]
    [HttpPost("logout", Name = "Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthConstant.RefreshToken);
        return Ok(Result.Success(new Success(AuthMessage.LogoutSuccessfully.GetMessage().Code,
            AuthMessage.LogoutSuccessfully.GetMessage().Message)));
    }
}
