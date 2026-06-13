using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Plugin;

namespace HidePlayers.Services;

public sealed class PlayerManager(
  IPluginContext pluginContext) : Dictionary<int, bool> {
  private Plugin.Plugin plugin = null!;

  private enum Action {
    CONNECT, DISCONNECT
  }

  public void Init(bool hotReload) {
    plugin = (pluginContext.Plugin as Plugin.Plugin)!;

    plugin.RegisterListener<Listeners.OnClientPutInServer>(slot
      => OnPlayerAction(slot, Action.CONNECT));

    plugin.RegisterListener<Listeners.OnClientDisconnect>(slot
      => OnPlayerAction(slot, Action.DISCONNECT));

    if (hotReload) {
      foreach (var player in Utilities.GetPlayers()) {
        AddPlayer(player);
      }
    }
  }

  private void OnPlayerAction(int slot, Action action) {
    var player = Utilities.GetPlayerFromSlot(slot);

    if (player == null || !player.IsValid) return;

    if (action == Action.CONNECT) {
      AddPlayer(player);
    } else {
      RemovePlayer(player);
    }
  }

  public bool AddPlayer(CCSPlayerController player, bool value = default)
    => TryAdd(player.Slot, value);

  public bool RemovePlayer(CCSPlayerController player) => Remove(player.Slot);
}
