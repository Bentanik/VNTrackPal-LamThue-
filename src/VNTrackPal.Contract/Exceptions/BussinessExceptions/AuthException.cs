using VNTrackPal.Contract.Common.Messages;

namespace VNTrackPal.Contract.Exceptions.BussinessExceptions;

public static class AuthException
{
    public sealed class UserExistException : BadRequestException
    {
        public UserExistException()
                : base(AuthMessage.EmailExistException.GetMessage().Message,
                    AuthMessage.EmailExistException.GetMessage().Code)
        { }
    }

    public sealed class UserNotExistException : NotFoundException
    {
        public UserNotExistException()
                : base(AuthMessage.EmailNotExistException.GetMessage().Message,
                    AuthMessage.EmailNotExistException.GetMessage().Code)
        { }
    }

    public sealed class PasswordNotMatchException : BadRequestException
    {
        public PasswordNotMatchException()
                : base(AuthMessage.PasswordNotMatchException.GetMessage().Message,
                    AuthMessage.PasswordNotMatchException.GetMessage().Code)
        { }
    }

    public sealed class TokenPasswordExpiredException : BadRequestException
    {
        public TokenPasswordExpiredException()
                : base(AuthMessage.TokenPasswordExpiredException.GetMessage().Message,
                    AuthMessage.TokenPasswordExpiredException.GetMessage().Code)
        { }
    }

    public sealed class UserBannedException : BadRequestException
    {
        public UserBannedException()
                : base(AuthMessage.UserBannedException.GetMessage().Message,
                    AuthMessage.UserBannedException.GetMessage().Code)
        { }
    }

    public sealed class LoginTokenExpiredException : AuthorizeException
    {
        public LoginTokenExpiredException()
                : base(AuthMessage.LoginTokenExpiredException.GetMessage().Message,
                    AuthMessage.LoginTokenExpiredException.GetMessage().Code)
        { }
    }
}
