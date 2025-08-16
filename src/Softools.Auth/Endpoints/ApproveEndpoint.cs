using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Softools.Auth.Endpoints;

public class ApproveEndpoint : EndpointWithoutRequest
{
    private readonly AuthDbContext _context;

    public ApproveEndpoint(AuthDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/approve/{Id}");
        Policies("superuser");
        Description(x => x
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("Id");

        if (id == Guid.Empty)
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
            return;
        }

        var user = await _context.UserCredentials
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: ct);
        if (user is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        user.IsApproved = true;
        _context.UserCredentials.Update(user);
        await _context.SaveChangesAsync(ct);
        await Send.NoContentAsync(ct);
    }
}