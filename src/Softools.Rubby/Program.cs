using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Softools.Rubby;

var services = ConfigureServices();

var client = services.GetRequiredService<DiscordSocketClient>();
var interactionService = services.GetRequiredService<InteractionService>();

var clientLogger = services.GetRequiredService<ILogger<DiscordSocketClient>>();
var interactionLogger = services.GetRequiredService<ILogger<InteractionService>>();

client.Log += (msg) =>
{
    Log(msg, clientLogger);
    return Task.CompletedTask;
};

interactionService.Log += (msg) =>
{
    Log(msg, interactionLogger);
    return Task.CompletedTask;
};

client.InteractionCreated += async (interaction) =>
{
    var ctx = new SocketInteractionContext(client, interaction);
    await interactionService.ExecuteCommandAsync(ctx, services);
};

await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("BOT_TOKEN") ?? throw new InvalidOperationException("BOT_TOKEN environment variable is not set."));
await client.StartAsync();

client.Ready += async () =>
{
    await interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), services);
    await interactionService.RegisterCommandsGloballyAsync();
    clientLogger.LogInformation("Bot is ready and commands are registered globally.");
};

await Task.Delay(Timeout.Infinite);
return;

static void Log(LogMessage message, ILogger logger)
{
    switch (message.Severity)
    {
        case LogSeverity.Critical:
            logger.LogCritical(message.Exception, message.Message);
            break;
        case LogSeverity.Error:
            logger.LogError(message.Exception, message.Message);
            break;
        case LogSeverity.Warning:
            logger.LogWarning(message.Exception, message.Message);
            break;
        case LogSeverity.Info:
            logger.LogInformation(message.Message);
            break;
        case LogSeverity.Verbose:
            logger.LogDebug(message.Message);
            break;
        case LogSeverity.Debug:
            logger.LogTrace(message.Message);
            break;
    }
}

static ServiceProvider ConfigureServices()
{
    return new ServiceCollection()
        .AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        })
        .AddSingleton<DiscordSocketClient>(provider =>
            new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
            }))
        .AddSingleton<InteractionService>(provider =>
            new InteractionService(provider.GetRequiredService<DiscordSocketClient>()))
        .AddSingleton<PingCommand>()
        .BuildServiceProvider();
}