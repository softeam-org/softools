using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Documentos;
using Softools.Documentos.Services;
using Softools.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<TemplateService>();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument();

builder.Services.AddDbContext<DocumentosDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("documentosdb")));

var app = builder.Build();

// Middlewares
app.UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "documentos/openapi/{documentName}.json");
    app.MapScalarApiReference("/documentos/docs", c =>
    {
        c.OpenApiRoutePattern = "documentos/openapi/{documentName}.json";
    });
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DocumentosDbContext>();
    dbContext.Database.Migrate();
}

app.Run();