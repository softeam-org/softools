using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Softools.Rubby;
using Softools.Rubby.Commands;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddLogging(b => b.AddConsole());
        services.AddHostedService<BotService>();
        services.AddSingleton<DiscordSocketClient>(_ =>
            new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
            }));
        services.AddSingleton<InteractionService>(provider =>
            new InteractionService(provider.GetRequiredService<DiscordSocketClient>()));
        services.AddSingleton<PingCommand>();
        services.AddSingleton<CreateTrackerCommand>();
        services.AddDbContext<RubbyDbContext>();
    })
    .Build();

using var scope = host.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<RubbyDbContext>();

await db.Database.MigrateAsync();


await host.RunAsync();