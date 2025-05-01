namespace brawlstars.Brawlstars;

public record struct BrawlerActionCategory(int Value, string Name) {
    public static readonly BrawlerActionCategory None = new(0, nameof(None));
    public static readonly BrawlerActionCategory One = new(1, nameof(One));
    public static readonly BrawlerActionCategory Two = new(2, nameof(Two));
    public static readonly BrawlerActionCategory Three = new(3, nameof(Three));

    public static bool operator <(int left, BrawlerActionCategory right) {
        return left < right.Value;
    }

    public static bool operator >(int left, BrawlerActionCategory right) {
        return left > right.Value;
    }
}