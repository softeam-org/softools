using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Softools.Rubby.Entities;

namespace Softools.Rubby;

public class BotService : BackgroundService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;
    private readonly ILogger<BotService> _logger;

    public BotService(
        DiscordSocketClient client,
        InteractionService interactionService,
        IServiceProvider services,
        ILogger<BotService> logger)
    {
        _client = client;
        _interactionService = interactionService;
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        WireUpEvents();

        var token = Environment.GetEnvironmentVariable("BOT_TOKEN")
            ?? throw new InvalidOperationException("BOT_TOKEN environment variable is not set.");

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Keep running until the host is stopping
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException) { /* Expected when stopping */ }
    }

    private void WireUpEvents()
    {
        _client.Log += msg => LogAsync(msg, nameof(DiscordSocketClient));
        _interactionService.Log += msg => LogAsync(msg, nameof(InteractionService));

        _client.Ready += OnReadyAsync;
        _client.InteractionCreated += async interaction =>
        {
            var ctx = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, _services);
        };
    }

    private async Task OnReadyAsync()
{
    await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
    await _interactionService.RegisterCommandsGloballyAsync();
    _logger.LogInformation("Bot is ready and commands are registered globally.");

    _ = Task.Run(async () =>
    {
        while (true)
        {
            try
            {
                _logger.LogInformation("Updating server metrics tracker message...");
                using var scope = _services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<RubbyDbContext>();

                var delayEntry = await db.ConfigKeys
                    .FirstOrDefaultAsync(c => c.Key == ConfigKey.ServerMetricsUpdateDelay);

                var delaySeconds = 30; // default
                if (delayEntry != null && int.TryParse(delayEntry.Value, out var parsedDelay) && parsedDelay > 0)
                {
                    delaySeconds = parsedDelay;
                }

                var messageEntry = await db.ConfigKeys
                    .FirstOrDefaultAsync(c => c.Key == ConfigKey.ServerMetricsTrackerMessageId);
                var channelEntry = await db.ConfigKeys
                    .FirstOrDefaultAsync(c => c.Key == ConfigKey.ServerMetricsTrackerChannelId);

                if (messageEntry != null &&
                    channelEntry != null &&
                    ulong.TryParse(messageEntry.Value, out var messageId) &&
                    ulong.TryParse(channelEntry.Value, out var channelId))
                {
                    if (_client.GetChannel(channelId) is IMessageChannel channel &&
                        await channel.GetMessageAsync(messageId, CacheMode.AllowDownload) is IUserMessage message)
                    {
                        var newEmbed = await Utils.BuildContainerStatsEmbedAsync();
                        if (newEmbed != null)
                            await message.ModifyAsync(m => m.Embed = newEmbed);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tracker message.");
                await Task.Delay(TimeSpan.FromSeconds(30)); // fallback delay on error
            }
        }
    });
}


    private Task LogAsync(LogMessage message, string source)
    {
        var level = message.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error    => LogLevel.Error,
            LogSeverity.Warning  => LogLevel.Warning,
            LogSeverity.Info     => LogLevel.Information,
            LogSeverity.Verbose  => LogLevel.Debug,
            LogSeverity.Debug    => LogLevel.Trace,
            _                    => LogLevel.Information
        };

        _logger.Log(level, message.Exception, "[{Source}] {Message}", source, message.Message);
        return Task.CompletedTask;
    }
}
