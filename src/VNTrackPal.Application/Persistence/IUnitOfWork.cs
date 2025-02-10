using VNTrackPal.Application.Persistence.Repository;

namespace VNTrackPal.Application.Persistence;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
}