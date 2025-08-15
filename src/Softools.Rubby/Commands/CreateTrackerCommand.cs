using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Softools.Rubby.Entities;

namespace Softools.Rubby.Commands;

[Group("create-tracker", "Commands to create trackers.")]
public class CreateTrackerCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly RubbyDbContext _context;

    public CreateTrackerCommand(RubbyDbContext context)
    {
        _context = context;
    }

    [SlashCommand("server", "Create a new server metrics tracker.")]
    public async Task CreateServerMetricsTracker()
    {
        await RespondAsync("Creating server metrics tracker...", ephemeral: true);
        var embed = await Utils.BuildContainerStatsEmbedAsync();
        var msg = await Context.Channel.SendMessageAsync(embed: embed);

        await SaveOrUpdateConfigAsync(ConfigKey.ServerMetricsTrackerMessageId, msg.Id.ToString());
        await SaveOrUpdateConfigAsync(ConfigKey.ServerMetricsTrackerChannelId, msg.Channel.Id.ToString());

        await _context.SaveChangesAsync();
    }

    private async Task SaveOrUpdateConfigAsync(string key, string value)
    {
        var existing = await _context.ConfigKeys.FirstOrDefaultAsync(c => c.Key == key);
        if (existing is not null)
            existing.Value = value;
        else
            _context.ConfigKeys.Add(new ConfigKey { Key = key, Value = value });
    }
}