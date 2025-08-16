using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Auth.Entities;

namespace Softools.Auth.Endpoints;

public class GetPendingEndpoint : EndpointWithoutRequest<List<UserCredentials>>
{
    private readonly AuthDbContext _context;
    public GetPendingEndpoint(AuthDbContext context)
    {
        _context = context;
    }
    public override void Configure()
    {
        Get("pending");
        Policies("superuser");
        Description(x => x
            .WithTags("Auth")
            .Produces<List<UserCredentials>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized));
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var pendingUsers = await _context.UserCredentials
            .Where(u => !u.IsApproved)
            .ToListAsync(ct);

        if (pendingUsers == null || !pendingUsers.Any())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(pendingUsers, ct);
    }
}