using Terraria;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class Dust1 : BrawlerDust {
    public override string BrawlerName => "MEG";

    public override void OnSpawn(Dust dust) {
        dust.noGravity = true;
    }
}