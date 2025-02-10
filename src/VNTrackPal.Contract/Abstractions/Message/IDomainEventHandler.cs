using MediatR;

namespace VNTrackPal.Contract.Abstractions.Message;

public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
