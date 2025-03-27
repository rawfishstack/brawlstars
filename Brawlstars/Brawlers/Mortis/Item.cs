namespace brawlstars.Brawlstars.Brawlers.Mortis;

public sealed class Item : BrawlerItem {
    public Item() {
        Behavior = new Behavior(this);
    }

    public override string BrawlerName => "MORTIS";

    public override Rarity Rarity => Rarity.Mythic;
}