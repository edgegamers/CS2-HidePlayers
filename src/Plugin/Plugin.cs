using CounterStrikeSharp.API.Core;
using HidePlayers.Services;

namespace HidePlayers.Plugin;

public sealed partial class Plugin(
  PlayerManager playerManager,
  HideManager hideManager,
  CommandManager commandManager) : BasePlugin {
  public override string ModuleName { get; } = "HidePlayers";

  public override string ModuleAuthor { get; } = "xstage";

  public override string ModuleVersion { get; } = "1.3.2";

  public override void Load(bool hotReload) {
    playerManager.Init(hotReload);
    commandManager.Init();
    hideManager.Init();
  }
}
