namespace Softools.Auth.Entities;

public class UserCredentials
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public bool IsApproved { get; set; } = false;
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}