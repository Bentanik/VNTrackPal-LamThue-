namespace VNTrackPal.Contract.DTOs.AuthDTOs;
public record LoginDTO(
    AuthTokenDTO AuthTokenDTO,
    AuthUserDTO AuthUserDTO
);