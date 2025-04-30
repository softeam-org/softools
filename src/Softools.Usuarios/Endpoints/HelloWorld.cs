using FastEndpoints;

namespace Softools.Usuarios.Endpoints;

public class HelloWorld : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("usuarios/helloworld");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync("Hello World!", cancellation: ct);
    }
}