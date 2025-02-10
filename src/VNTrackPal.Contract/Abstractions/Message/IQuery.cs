using MediatR;
using VNTrackPal.Contract.Abstractions.Shared;

namespace VNTrackPal.Contract.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}