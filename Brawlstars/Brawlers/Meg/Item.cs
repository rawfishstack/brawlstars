namespace brawlstars.Brawlstars.Brawlers.Meg;

public sealed class Item : BrawlerItem {
    public Item() {
        Behavior = new Behavior(this);
    }

    public override string BrawlerName => "MEG";

    public override Rarity Rarity => Rarity.Legendary;
}