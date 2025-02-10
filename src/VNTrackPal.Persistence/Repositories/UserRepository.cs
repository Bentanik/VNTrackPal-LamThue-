namespace VNTrackPal.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : RepositoryBase<User, Guid>(context), IUserRepository
{
}