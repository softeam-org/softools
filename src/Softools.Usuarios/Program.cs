using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Usuarios.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Aspire (inclui JWT, OpenTelemetry, Health Checks)
builder.AddServiceDefaults(); // <- Já inclui a configuração JWT do Extensions.cs

// 2. Configuração do FastEndpoints e Swagger
builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "API de Usuários";
            s.Version = "1.0";
        };


    });

// 3. Banco de Dados (PostgreSQL)
builder.Services.AddDbContext<UsuariosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("usuariosdb"));
});

var app = builder.Build();

// 4. Middlewares
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseFastEndpoints();

// 5. Documentação (Swagger + Scalar) - Apenas em Development
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(); // Padrão: /swagger/{documentName}/swagger.json
    app.UseSwaggerUi();
    app.MapScalarApiReference("/docs");
}

// 6. Migração do Banco de Dados Automática
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UsuariosDbContext>();
    dbContext.Database.Migrate();
}

app.Run();