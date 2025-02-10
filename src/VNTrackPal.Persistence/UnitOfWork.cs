using VNTrackPal.Application.Persistence;
using VNTrackPal.Application.Persistence.Repository;

namespace VNTrackPal.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context, IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
     => await _context.SaveChangesAsync();

    async ValueTask IAsyncDisposable.DisposeAsync()
        => await _context.DisposeAsync();

    public IUserRepository UserRepository { get; }

    public IRoleRepository RoleRepository { get; }
}

