namespace brawlstars.Brawlstars.Brawlers.Bibi;

public sealed class Item : BrawlerItem {
    public Item() {
        Behavior = new Behavior(this);
    }

    public override string BrawlerName => "BIBI";

    public override Rarity Rarity => Rarity.Epic;
}