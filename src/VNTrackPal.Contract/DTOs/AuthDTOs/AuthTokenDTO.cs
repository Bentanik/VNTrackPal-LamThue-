namespace VNTrackPal.Contract.DTOs.AuthDTOs;

public record AuthTokenDTO(
    string? AccessToken = null,
    string? RefreshToken = null,
    string? TokenType = null
);
