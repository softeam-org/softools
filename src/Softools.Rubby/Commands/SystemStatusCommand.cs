using Discord;
using Discord.Interactions;

namespace Softools.Rubby.Commands;

public class SystemStatusCommand : InteractionModuleBase
{
    [SlashCommand("systemstatus", "Show CPU, RAM, and Disk usage of the server")]
    public async Task SystemStatusAsync()
    {
        await DeferAsync();

        // CPU %
        double cpuUsage = await GetCpuUsagePercentAsync();

        // Memoria
        var memInfo = await File.ReadAllLinesAsync("/proc/meminfo");
        long memTotal = 0, memAvailable = 0;
        foreach (var line in memInfo)
        {
            if (line.StartsWith("MemTotal:"))
                memTotal = ParseKb(line);
            else if (line.StartsWith("MemAvailable:"))
                memAvailable = ParseKb(line);
        }

        // Disco
        var drive = new DriveInfo("/");
        var totalDisk = drive.TotalSize / (1024 * 1024);
        var freeDisk = drive.AvailableFreeSpace / (1024 * 1024);

        var embed = new EmbedBuilder()
            .WithTitle("Status do Sistema")
            .AddField("Uso de CPU", $"{cpuUsage:F2}%", true)
            .AddField("Memória", $"{memAvailable / 1024} MB livre / {memTotal / 1024} MB total", true)
            .AddField("Disco", $"{freeDisk} MB livre / {totalDisk} MB total", true)
            .WithColor(Color.DarkBlue)
            .WithTimestamp(DateTimeOffset.Now)
            .Build();

        await FollowupAsync(embed: embed);
    }
    
    /// <summary>
    /// Gambiarra para pegar o uso de CPU no Linux. Não é muito preciso, mas é o que temos.
    /// </summary>
    /// <param name="delayMs"></param>
    /// <returns></returns>
    private async Task<double> GetCpuUsagePercentAsync(int delayMs = 500)
    {
        long[] ReadCpuTimes()
        {
            var line = File.ReadLines("/proc/stat").FirstOrDefault(l => l.StartsWith("cpu "));
            if (line == null) return null;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Skip(1).Select(long.Parse).ToArray();
        }

        long[] cpuTimes1 = ReadCpuTimes();
        if (cpuTimes1 == null) return 0;

        await Task.Delay(delayMs);

        long[] cpuTimes2 = ReadCpuTimes();
        if (cpuTimes2 == null) return 0;

        long idle1 = cpuTimes1[3] + cpuTimes1[4]; 
        long idle2 = cpuTimes2[3] + cpuTimes2[4];

        long nonIdle1 = cpuTimes1[0] + cpuTimes1[1] + cpuTimes1[2] + cpuTimes1[5] + cpuTimes1[6] + cpuTimes1[7];
        long nonIdle2 = cpuTimes2[0] + cpuTimes2[1] + cpuTimes2[2] + cpuTimes2[5] + cpuTimes2[6] + cpuTimes2[7];

        long total1 = idle1 + nonIdle1;
        long total2 = idle2 + nonIdle2;

        long totald = total2 - total1;
        long idled = idle2 - idle1;

        if (totald == 0) return 0;

        double cpuPercentage = (double)(totald - idled) / totald * 100;

        return cpuPercentage;
    }


    private long ParseKb(string line)
    {
        var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && long.TryParse(parts[1], out var value))
            return value;
        return 0;
    }
}