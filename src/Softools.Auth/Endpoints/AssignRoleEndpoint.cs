using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Auth.Entities;
using Softools.Auth.Models;

namespace Softools.Auth.Endpoints;

public class AssignRoleEndpoint : Endpoint<AssignRoleRequest>
{
    private readonly AuthDbContext _context;
    public AssignRoleEndpoint(AuthDbContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Post("/auth/assign-role");
        Policies("superuser");
        Description(x => x
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized));
    }
    
    public override async Task HandleAsync(AssignRoleRequest req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.RoleName))
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        var user = await _context.UserCredentials
            .FirstOrDefaultAsync(u => u.Email == req.Email, cancellationToken: ct);
        
        if (user is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == req.RoleName, cancellationToken: ct);
        
        if (role is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        user.UserRoles.Add(new UserRole { RoleId = role.Id });
        _context.UserCredentials.Update(user);
        await _context.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
    
}