using Projects;
using Softools.AppHost;

var orquestrador = new Orquestrador();

orquestrador.AdicionarApi<Softools_Projetos>("projetos");

await orquestrador.Build().RunAsync();
