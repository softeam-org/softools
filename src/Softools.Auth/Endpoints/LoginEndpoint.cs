using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.EntityFrameworkCore;
using Softools.Auth.Models;

namespace Softools.Auth.Endpoints;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly AuthDbContext _context;
    
    public LoginEndpoint(AuthDbContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "User login";
            s.Description = "Endpoint for user login";
            s.Response<LoginResponse>(200, "Success");
            s.Response(400, "Bad Request");
            s.Response(401, "Unauthorized");
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _context.UserCredentials
            .FirstOrDefaultAsync(u => u.Email == req.Email, cancellationToken: ct);
        
        if (user is null)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(req.Password, user.Salt);
        if (hash != user.PasswordHash)
        {
            await Send.UnauthorizedAsync(ct);
            return;
        }
        
        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = "A secret token signing key";
            o.ExpireAt = DateTime.UtcNow.AddDays(1);
            o.User.Claims.Add(("Email", req.Email));
        });
        
        var response = new LoginResponse
        {
            Email = req.Email,
            Token = jwtToken
        };

        await Send.OkAsync(response, cancellation: ct);
        
    }

}