using MediatR;
using VNTrackPal.Contract.Abstractions.Shared;

namespace VNTrackPal.Contract.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
