using CounterStrikeSharp.API.Core;

namespace HidePlayers;

public class PluginConfig : IBasePluginConfig
{
    public string Command { get; set; } = "css_hidemodels,css_hide";
    public string Hidden { get; set; } = "@all";
    public bool HideDead { get; set; } = true;

    public int Version { get; set; } = 2;
}