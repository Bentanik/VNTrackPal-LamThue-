namespace VNTrackPal.Contract.Common.Messages;

public enum AuthMessage
{
    [Message("Email này đã tồn tại", "auth01")]
    EmailExistException,

    [Message("Email này chưa tồn tại", "auth02")]
    EmailNotExistException,

    [Message("Đăng ký thành công, xác nhận tài khoản được gửi vào mail", "auth03")]
    RegisterSuccessfully,

    [Message("Mật khẩu không trùng khớp", "auth04")]
    PasswordNotMatchException,

    [Message("Đăng nhập thành công", "auth05")]
    LoginSuccessfully,

    [Message("Xin vui lòng vô Gmail để kiểm tra", "auth06")]
    ConfirmForgotPasswordSuccessfully,

    [Message("Đã hết thời gian xóa mật khẩu, xin vui lòng thử lại", "auth07")]
    TokenPasswordExpiredException,

    [Message("Tài khoản này đã bị cấm", "auth08")]
    UserBannedException,

    [Message("Thay đổi mật khẩu thành công", "auth09")]
    ChangePasswordSuccessfully,

    [Message("Refresh Token thành công", "auth10")]
    RefreshTokenSuccessfully,

    [Message("Hết thời gian đăng nhập, xin vui lòng đăng nhập lại", "auth11")]
    LoginTokenExpiredException,

    [Message("Đăng xuất thành công", "auth12")]
    LogoutSuccessfully,
}

