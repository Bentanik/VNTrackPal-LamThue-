using VNTrackPal.Contract.Common.Enums;

namespace VNTrackPal.Persistence.DependencyInjection.Options;

public class InitialData
{
    public static IEnumerable<Role> GetRoles()
    {
        return
        [
            Role.Create(Guid.NewGuid(), RoleEnum.Admin.ToString()),
            Role.Create(Guid.NewGuid(), RoleEnum.Member.ToString()),
        ];
    }
}
