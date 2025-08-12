using Discord.Interactions;

namespace Softools.Rubby;

public class PingCommand : InteractionModuleBase
{
    
    [SlashCommand("ping", "Returns a pong response.")]  
    public async Task PingAsync()
    {
        await RespondAsync("Pong!");
    }
    
}