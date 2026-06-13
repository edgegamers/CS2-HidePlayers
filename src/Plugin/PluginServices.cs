using CounterStrikeSharp.API.Core;
using HidePlayers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HidePlayers.Plugin;

public sealed class PluginServices : IPluginServiceCollection<Plugin>
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<PlayerManager>();
        serviceCollection.AddSingleton<CommandManager>();
        serviceCollection.AddSingleton<HideManager>();
    }
}