using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace brawlstars.Brawlstars.Common.Configs;

public class ServerConfig : ModConfig {
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [Header("Balances")]
    [DefaultValue(false)]
    public bool DamageValueNormalization;

    [Header("Developer")]
    [DefaultValue(false)]
    public bool DebugMode;

    public override void OnChanged() {
        ServerDebugMode = DebugMode;
    }

    public static bool ServerDebugMode { get; private set; }
}