using System.Security.Claims;
using FastEndpoints.Security;
using Softools.Auth.Entities;

namespace Softools.Auth;

public static class JwtHelper
{
    public static string GenerateToken(UserCredentials user, TimeSpan? expiresIn = null)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")
                     ?? throw new InvalidOperationException(
                         "JWT Secret não configurado. Defina uma variável de ambiente JWT_SECRET ou configure."
                     );

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = secret;
            o.ExpireAt = DateTime.UtcNow.Add(expiresIn ?? TimeSpan.FromDays(1));

            o.User.Claims.Add(("Email", user.Email));

            foreach (var role in user.UserRoles.Select(ur => ur.Role.Name))
            {
                o.User.Roles.Add(role);
            }
        });

        return jwtToken;
    }
}
