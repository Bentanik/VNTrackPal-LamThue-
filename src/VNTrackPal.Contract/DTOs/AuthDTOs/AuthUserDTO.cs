namespace VNTrackPal.Contract.DTOs.AuthDTOs;
public record AuthUserDTO(
    string? Email = null,
    string? FullName = null,
    string? AvatarUrl = null
);

