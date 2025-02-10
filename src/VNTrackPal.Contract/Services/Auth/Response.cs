using VNTrackPal.Contract.DTOs.AuthDTOs;

namespace VNTrackPal.Contract.Services.Auth;

public static class Response
{
    public record LoginResponse
        (LoginDTO LoginDto);
}
