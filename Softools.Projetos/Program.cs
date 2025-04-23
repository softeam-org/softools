using FastEndpoints;
using FastEndpoints.Swagger;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument();

var app = builder.Build();

// Middlewares
app.UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "projetos/openapi/{documentName}.json");
    app.MapScalarApiReference("/projetos/docs", c =>
    {
        c.OpenApiRoutePattern = "projetos/openapi/{documentName}.json";
    });
}

app.UseHttpsRedirection();

app.Run();
