using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Auth;
using Softools.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument();
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("authdb"));
});

var app = builder.Build();
app.UseServiceDefaults();

// Middlewares
app.UseAuthentication().UseAuthorization().UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "auth/openapi/{documentName}.json");
    app.MapScalarApiReference("/docs", c =>
    {
        c.OpenApiRoutePattern = "auth/openapi/{documentName}.json";
    });
}

// Aplicar migrações automaticamente
// Causará erro pois não temos migrations configuradas ainda.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    dbContext.Database.Migrate();
}
app.UseHttpsRedirection();

app.Run();