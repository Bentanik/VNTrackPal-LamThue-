namespace VNTrackPal.Contract.DTOs.UserDTOs;

public record UserDto(Guid? Id = null, string? Email = null, string? Password = null, string? FullName = null, string? Avatar = null);