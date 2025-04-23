using Projects;
using Softools.AppHost;

var orquestrador = new Orquestrador();

orquestrador.AdicionarApi<Softools_Projetos>("projetos", Recursos.BancoDeDadosPostgreSQL);

await orquestrador.Build().RunAsync();
