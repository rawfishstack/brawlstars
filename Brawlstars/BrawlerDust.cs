using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public abstract class BrawlerDust : ModDust {
    /// <summary>
    /// 英雄名称。
    /// 全部大写。
    /// </summary>
    public abstract string BrawlerName { get; }
    public override string Name => BrawlerName + GetType().Name;
    public override string Texture => (GetType().Namespace + "/Texture/").Replace('.', '/') + GetType().Name;
}