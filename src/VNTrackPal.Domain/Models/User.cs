using System.Text.Json.Serialization;
using VNTrackPal.Domain.Abstractions;

namespace VNTrackPal.Domain.Models;

public class User : DomainEntity<Guid>
{
    public string Email { get; private set; } = default!;
    public string Password { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public bool IsActive { get; private set; } = default!;
    public string? PublicAvatarId { get; private set; }
    public string? PublicAvatarUrl { get; private set; }

    public Guid RoleId { get; private set; } = default;
    [JsonIgnore]
    public Role Role { get; set; } = default!;
    public static User Create(Guid Id, string email, string password, string fullName, Guid roleId, string? publicMediaId = null, string? publicMediaUrl = null)
    {
        return new User
        {
            Id = Id,
            Email = email,
            Password = password,
            FullName = fullName,
            RoleId = roleId,
            IsActive = false,
            IsDeleted = false,
            PublicAvatarId = publicMediaId,
            PublicAvatarUrl = publicMediaUrl
        };
    }

    public void Update(string? password = null)
    {
        if (password != null) Password = password;
    }
}
