using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Plugin;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Extensions;
using Microsoft.Extensions.Logging;

namespace HidePlayers.Services;

public sealed class CommandManager(
  ILogger<CommandManager> logger,
  IPluginContext pluginContext,
  HideManager hideManager) {
  private Plugin.Plugin plugin = null!;

  public void Init() {
    plugin = (pluginContext.Plugin as Plugin.Plugin)!;

    var cmds = plugin.Config.Commands.Split(';')
     .Select(c => c.Trim())
     .ToArray();

    foreach (var cmd in cmds) {
      plugin.AddCommand(cmd, "Hide players models", OnToggleCommand);
    }

    plugin.AddCommand("css_hide_reload",
      $"Reload `{plugin.ModuleName}` configuration", OnConfigReload);
  }

  [RequiresPermissions("@css/root")]
  private void OnConfigReload(CCSPlayerController? player,
    CommandInfo commandInfo) {
    try {
      plugin.Config.Reload();
      plugin.Config.Mode = plugin.ParseHideMode(plugin.Config.WhoHidden);

      commandInfo.ReplyToCommand(
        $"[{plugin.ModuleName}] You have successfully reloaded the config.");
    } catch (Exception ex) {
      commandInfo.ReplyToCommand($"[{plugin.ModuleName}] An error occurred.");

      logger.LogError(
        "An error occurred while reloading the configuration: {error}",
        ex.Message);
    }
  }

  private void OnToggleCommand(CCSPlayerController? player,
    CommandInfo commandInfo) {
    if (player == null) return;

    string tag = plugin.Localizer["Plugin.Tag"];

    string status =
      plugin.Localizer[
        hideManager.Toggle(player) ? "Plugin.Enable" : "Plugin.Disable"];

    player.PrintToChat(plugin.Localizer["Player.Hide", tag, status]);
  }
}
