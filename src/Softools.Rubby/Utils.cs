using Docker.DotNet;
using Docker.DotNet.Models;
using Discord;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Softools.Rubby;

public static partial class Utils
{
    public static async Task<Embed> BuildContainerStatsEmbedAsync()
    {
        var docker = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
        var containers = await docker.Containers.ListContainersAsync(new ContainersListParameters { All = false });

        var embed = new EmbedBuilder()
            .WithTitle("🐳 Metricas dos Containers")
            .WithColor(Color.Teal)
            .WithCurrentTimestamp()
            .WithFooter("🦆 Rubby");

        foreach (var container in containers)
        {
            try
            {
                var stats = await docker.Containers.GetContainerStatsAsync(container.ID,
                    new ContainerStatsParameters { Stream = false }, CancellationToken.None);
                using var reader = new System.IO.StreamReader(stats);
                var statsJson = await reader.ReadToEndAsync();
                var statsData = JsonDocument.Parse(statsJson).RootElement;

                // CPU usage calculation
                var cpuDelta = statsData.GetProperty("cpu_stats").GetProperty("cpu_usage")
                                   .GetProperty("total_usage").GetInt64() -
                               statsData.GetProperty("precpu_stats").GetProperty("cpu_usage")
                                   .GetProperty("total_usage").GetInt64();
                var systemCpuDelta = statsData.GetProperty("cpu_stats").GetProperty("system_cpu_usage").GetInt64() -
                                     statsData.GetProperty("precpu_stats").GetProperty("system_cpu_usage")
                                         .GetInt64();
                var cpuUsage = (double)cpuDelta / systemCpuDelta *
                               statsData.GetProperty("cpu_stats").GetProperty("online_cpus").GetInt32() * 100.0;

                var memoryUsage = statsData.GetProperty("memory_stats").GetProperty("usage").GetInt64();
                var memoryLimit = statsData.GetProperty("memory_stats").GetProperty("limit").GetInt64();
                var memoryUsageMB = memoryUsage / (1024.0 * 1024.0);
                var memoryLimitMB = memoryLimit / (1024.0 * 1024.0);
                var memoryLimitGB = memoryLimit / (1024.0 * 1024.0 * 1024.0);
                var memoryPercent = (double)memoryUsage / memoryLimit * 100.0;

                // Clean container 
                string name = container.Names.FirstOrDefault()?.TrimStart('/') ?? container.ID.Substring(0, 12);

                name = CleanContainerNameRegex().Replace(name, "");


                embed.AddField(
                    $"**{name}**",
                    $"• Status: `{container.State}`\n" +
                    $"• CPU: `{cpuUsage:F2}%`\n" +
                    $"• Memoria: `{memoryUsageMB:F0}MB / {memoryLimitGB:F2}GB ({memoryPercent:F2}%)`",
                    inline: true);
            }
            catch (Exception ex)
            {
                string name = container.Names.FirstOrDefault()?.TrimStart('/') ?? container.ID.Substring(0, 12);
                name = System.Text.RegularExpressions.Regex.Replace(name, @"\d+$", "");
                embed.AddField(
                    $"**{name}**",
                    $"⚠ Erro: `{ex.Message}`",
                    inline: true);
            }
        }

        return embed.Build();
    }
    
    [GeneratedRegex(@"^.*?-|-\d+$")]
    private static partial Regex CleanContainerNameRegex();
}