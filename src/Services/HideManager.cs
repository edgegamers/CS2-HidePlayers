using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Plugin;
using HidePlayers.Data;

namespace HidePlayers.Services;

public sealed class HideManager(
  IPluginContext pluginContext,
  PlayerManager playerManager) {
  private Plugin.Plugin plugin = null!;

  public void Init() {
    plugin = (pluginContext.Plugin as Plugin.Plugin)!;

    plugin.RegisterListener<Listeners.CheckTransmit>(OnCheckTransmit);
  }

  private void OnCheckTransmit(CCheckTransmitInfoList infoList) {
    foreach (var (info, player) in infoList) {
      if (player == null || player.Connected != PlayerConnectedState.Connected
        || !playerManager.TryGetValue(player.Slot, out var isEnableHide))
        continue;

      foreach (var slot in playerManager.Keys) {
        var target = Utilities.GetPlayerFromSlot(slot);

        if (target == null || !target.IsValid) continue;

        var targetPawn = target.PlayerPawn.Value;

        if (targetPawn == null) continue;

        if (targetPawn.LifeState != (byte)LifeState_t.LIFE_DEAD
          || targetPawn.LifeState != (byte)LifeState_t.LIFE_DYING) {
          if (target.Slot == player.Slot) continue;

          if (player.Pawn.Value?.As<CCSPlayerPawnBase>().PlayerState
            == CSPlayerState.STATE_OBSERVER_MODE)
            continue;
        }

        if (isEnableHide && (plugin.Config.Mode == HideMode.ALL
          || plugin.Config.Mode == HideMode.TEAM && player.Team == target.Team
          || plugin.Config.Mode == HideMode.ENEMY
          && player.Team != target.Team)) {
          info.TransmitEntities.Remove(targetPawn.Index);
        }
      }
    }
  }

  public bool Toggle(CCSPlayerController player) {
    return playerManager[player.Slot] ^= true;
  }
}
