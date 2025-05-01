namespace brawlstars.Brawlstars;

public record struct BrawlerActionAggregate(string Name) {
    public static readonly BrawlerActionAggregate Hero = new(nameof(Hero));
    public static readonly BrawlerActionAggregate Gadget = new(nameof(Gadget));
    public static readonly BrawlerActionAggregate StarPower = new(nameof(StarPower));
}