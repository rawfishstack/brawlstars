using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace brawlstars.Brawlstars.Common.Configs;

public class ClientConfig : ModConfig {
    public override ConfigScope Mode => ConfigScope.ClientSide;
    

    [Header("Graphics")]
    [DefaultValue(RenderQualityEnum.High)]
    public RenderQualityEnum RenderQuality;
    
    public enum RenderQualityEnum {
        Low = 0,
        Medium = 1,
        High = 2
    }
    
    [Header("Developer")]
    [DefaultValue(false)]
    public bool DebugMode;
}