using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Projetos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument();
builder.Services.AddDbContext<ProjetosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("projetosdb"));
});

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

// Aplicar migrações automaticamente
// Causará erro pois não temos migrations configuradas ainda.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjetosDbContext>();
    dbContext.Database.Migrate();
}
app.UseHttpsRedirection();

app.Run();
