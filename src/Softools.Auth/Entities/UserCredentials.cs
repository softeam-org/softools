namespace Softools.Auth.Entities;

public class UserCredentials
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
}