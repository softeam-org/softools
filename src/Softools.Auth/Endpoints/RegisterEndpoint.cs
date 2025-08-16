using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity.Data;
using Softools.Auth.Entities;

namespace Softools.Auth.Endpoints;

public class RegisterEndpoint : Endpoint<RegisterRequest>
{
    private readonly AuthDbContext _context;

    public RegisterEndpoint(AuthDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/register");
        AllowAnonymous();
        Description(x => x
            .WithName("Register")
            .Produces<string>(201)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(req.Password, salt);
        var user = new UserCredentials
        {
            Email = req.Email,
            PasswordHash = passwordHash,
            Salt = salt
        };

        _context.UserCredentials.Add(user);
        await _context.SaveChangesAsync(ct);
        var jwtToken = JwtHelper.GenerateToken(user);

        await Send.OkAsync(
            new
            {
                req.Email,
                Token = jwtToken
            }, ct);
    }
}