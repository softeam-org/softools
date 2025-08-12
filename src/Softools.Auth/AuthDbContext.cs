using Microsoft.EntityFrameworkCore;
using Softools.Auth.Entities;

namespace Softools.Auth;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserCredentials> UserCredentials { get; set; }
}