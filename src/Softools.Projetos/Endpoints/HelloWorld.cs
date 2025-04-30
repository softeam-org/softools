using FastEndpoints;

namespace Softools.Projetos.Endpoints;

public class HelloWorld : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("projetos/helloworld");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync("Hello World!", cancellation: ct);
    }
}