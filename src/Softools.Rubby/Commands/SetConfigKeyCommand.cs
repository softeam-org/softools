using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using Softools.Rubby.Entities;

namespace Softools.Rubby.Commands;

[Group("config", "Commands to manage configuration keys.")]
[RequireUserPermission(GuildPermission.Administrator)]
public class SetConfigKeyCommand : InteractionModuleBase
{
    private static readonly string[] _validKeys;
    private readonly RubbyDbContext _context;
    
    public SetConfigKeyCommand(RubbyDbContext context)
    {
        _context = context;
    }

    static SetConfigKeyCommand()
    {
        _validKeys = typeof(ConfigKey).GetFields()
            .Where(f => f.IsStatic && f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null))
            .ToArray();
    }
    
    [SlashCommand("set", "Set a configuration key to a value.")]
    public async Task SetConfigKeyAsync(
        [Summary("key", "The configuration key to set.")] string key,
        [Summary("value", "The value to set for the key.")] string value)
    {
        if (!_validKeys.Contains(key))
        {
            await RespondAsync($"Invalid key. Valid keys are: {string.Join(", ", _validKeys)}", ephemeral: true);
            return;
        }

        var existing = await _context.ConfigKeys.FirstOrDefaultAsync(c => c.Key == key);
        if (existing is not null)
        {
            existing.Value = value;
        }
        else
        {
            _context.ConfigKeys.Add(new ConfigKey { Key = key, Value = value });
        }

        await _context.SaveChangesAsync();
        await RespondAsync($"Configuration key `{key}` set to `{value}`.", ephemeral: true);
    }
    
    [SlashCommand("get", "Get the value of a configuration key.")]
    public async Task GetConfigKeyAsync(
        [Summary("key", "The configuration key to retrieve.")] string key)
    {
        if (!_validKeys.Contains(key))
        {
            await RespondAsync($"Invalid key. Valid keys are: {string.Join(", ", _validKeys)}", ephemeral: true);
            return;
        }

        var config = await _context.ConfigKeys.FirstOrDefaultAsync(c => c.Key == key);
        if (config is null)
        {
            await RespondAsync($"Configuration key `{key}` not found.", ephemeral: true);
            return;
        }

        await RespondAsync($"Configuration key `{key}` has value `{config.Value}`.", ephemeral: true);
    }
}