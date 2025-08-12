using Discord;
using Discord.Interactions;

namespace Softools.Rubby.Commands;

public class HealthCheckCommand : InteractionModuleBase
{
    private static readonly HttpClient _httpClient = new();

    [SlashCommand("healthcheck", "Check the health status of all services.")]
    public async Task HealthCheckAsync()
    {
        await DeferAsync();

        var services = new Dictionary<string, string>
        {
            { "Auth", "http://nginx/auth/health" },
            { "Documentos", "http://nginx/api/documents/health" },
            { "Projetos", "http://nginx/api/projetos/health" },
            { "Usuarios", "http://nginx/api/usuarios/health" }
        };


        var embedBuilder = new EmbedBuilder()
            .WithTitle("Service Health Check")
            .WithColor(Color.Green);

        foreach (var (name, url) in services)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadAsStringAsync();
                    embedBuilder.AddField(name, $"✅ Healthy: {status}", inline: true);
                }
                else
                {
                    embedBuilder.AddField(name, $"❌ Unhealthy (Status {response.StatusCode})", inline: true);
                }
            }
            catch (Exception ex)
            {
                embedBuilder.AddField(name, $"⚠️ Error: {ex.Message}", inline: true);
            }
        }

        await FollowupAsync(embed: embedBuilder.Build());
    }
}