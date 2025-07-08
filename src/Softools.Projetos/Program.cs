using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Softools.Projetos;
using Softools.Projetos.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Setup para integração com Aspire
builder.AddServiceDefaults();

// Config do servidor
builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.DocumentName = "v1";
            s.Title = "API de Projetos";
            s.Version = "1.0";
        };
    });


builder.Services.AddDbContext<ProjetosDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("projetosdb"));
});

var app = builder.Build();



// Middlewares
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseFastEndpoints();

// Documentação
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.MapScalarApiReference("projetos/docs");
}


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjetosDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
