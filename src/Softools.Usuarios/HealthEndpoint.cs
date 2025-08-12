using FastEndpoints;

namespace Softools.Usuarios;

public class HealthEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/health");
        AllowAnonymous();
        Description(b => b.WithTags("Health"));
    }
    
    public override Task HandleAsync(CancellationToken ct)
    {
        return SendAsync("Alive", cancellation: ct);
    }
}