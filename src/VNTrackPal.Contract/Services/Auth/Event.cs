using VNTrackPal.Contract.DTOs.UserDTOs;

namespace VNTrackPal.Contract.Services.Auth;

public static class Event
{
    public record UserRegisterdEvent(Guid Id, UserDto UserDto) : IDomainEvent;
}