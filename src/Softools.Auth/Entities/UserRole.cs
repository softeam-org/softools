namespace Softools.Auth.Entities;

public class UserRole
{
    public Guid UserId { get; set; }
    public UserCredentials User { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}