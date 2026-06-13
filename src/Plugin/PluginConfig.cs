using CounterStrikeSharp.API.Core;
using HidePlayers.Data;
using Microsoft.Extensions.Logging;

namespace HidePlayers.Plugin;

public sealed partial class Plugin : IPluginConfig<PluginConfig>
{
    public PluginConfig Config { get; set; } = new();

    public void OnConfigParsed(PluginConfig config)
    {
        if (config.Version < Config.Version)
        {
            Logger.LogWarning("Your config version is outdated. (v. {old} -> v. {new})", config.Version, Config.Version);
        }

        Config = config;
        config.Mode = ParseHideMode(config.WhoHidden);
    }

    public HideMode ParseHideMode(string hideMode) => hideMode switch
    {
        "@all" => HideMode.ALL,
        "@team" => HideMode.TEAM,
        "@enemy" => HideMode.ENEMY,
        _ => HideMode.ALL
    };
}