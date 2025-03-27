using Terraria;

namespace brawlstars.Brawlstars.Brawlers.Bibi;

public class Dust1 : BrawlerDust {
    public override string BrawlerName => "BIBI";

    public override void OnSpawn(Dust dust) {
        dust.noGravity = true;
    }
}