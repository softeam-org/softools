using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Usuarios.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument();
builder.Services.AddDbContext<UsuariosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("usuariosdb"));
});

var app = builder.Build();

// Middlewares
app.UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "usuarios/openapi/{documentName}.json");
    app.MapScalarApiReference("/usuarios/docs", c =>
    {
        c.OpenApiRoutePattern = "usuarios/openapi/{documentName}.json";
    });
}


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
    dbContext.Database.Migrate();
}
app.UseHttpsRedirection();

app.Run();