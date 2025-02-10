using MediatR;
using VNTrackPal.Contract.Abstractions.Shared;

namespace VNTrackPal.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}