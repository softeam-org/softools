using Projects;
using Softools.AppHost;

var orquestrador = new Orquestrador();

orquestrador.AdicionarApi<Softools_Projetos>("projetos", Recursos.BancoDeDadosPostgreSQL)
    .AdicionarApi<Softools_Usuarios>("usuarios", Recursos.BancoDeDadosPostgreSQL)
    .AdicionarApi<Softools_Auth>("auth", Recursos.BancoDeDadosPostgreSQL)
    .AdicionarAppNpm("website", @"../softools.website", "dev")
    .AdicionarApiGateway<Softools_Gateway>("gateway");

await orquestrador.Build().RunAsync();
