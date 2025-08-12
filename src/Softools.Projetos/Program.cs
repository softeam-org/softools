using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Projetos;
using Softools.Projetos.Data;
using Softools.ServiceDefaults;

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

app.UseAuthentication(); 
app.UseAuthorization();
app.UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "projetos/openapi/{documentName}.json");
    app.MapScalarApiReference("/docs", c =>
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
