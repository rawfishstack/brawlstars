namespace brawlstars.Brawlstars;

public class BrawlerActionType {
    public static readonly BrawlerActionType Static = new(BrawlerActionAggregate.Hero, nameof(Static));
    public static readonly BrawlerActionType Attack = new(BrawlerActionAggregate.Hero, nameof(Attack));
    public static readonly BrawlerActionType Super = new(BrawlerActionAggregate.Hero, nameof(Super));
    public static readonly BrawlerActionType Gadget = new(BrawlerActionAggregate.Gadget, nameof(Gadget));
    public static readonly BrawlerActionType StarPower = new(BrawlerActionAggregate.StarPower, nameof(StarPower));
    public static readonly BrawlerActionType HyperCharge = new(BrawlerActionAggregate.Hero, nameof(HyperCharge));
    public static readonly BrawlerActionType[] All = [Static, Attack, Super, Gadget, StarPower, HyperCharge];

    private BrawlerActionType(BrawlerActionAggregate aggregate, string name) { 
        Aggregate = aggregate;
        Name = name;
    }

    public readonly BrawlerActionAggregate Aggregate;
    public readonly string Name;

    public override string ToString() => Name;
}