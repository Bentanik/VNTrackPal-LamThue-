using VNTrackPal.Contract.DTOs.UserDTOs;

namespace VNTrackPal.Application.UseCases.V1.Events;

public sealed class UserRegisteredEventHandler
    (IEmailService emailService)
    : IDomainEventHandler<Contract.Services.Auth.Event.UserRegisterdEvent>
{
    public async Task Handle(Contract.Services.Auth.Event.UserRegisterdEvent notification, CancellationToken cancellationToken)
    {
       
       // Runs asynchronous, which can replace state worker or message broker
        Task.Run(() => SendNotificationAsync(notification.UserDto));
    }

    private async Task SendNotificationAsync(UserDto userDto)
    {
        await emailService.SendMailAsync(userDto.Email, "Đăng ký thành công", "UserCreatedEmail.html",
        new Dictionary<string, string> {
              {"ToEmail", userDto.Email!},
              {"UserId", userDto.Id.ToString()!},
         });
    }
}
