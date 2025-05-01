namespace brawlstars.Brawlstars;

public record struct BrawlerActionSlot(int Value) {
    public static readonly BrawlerActionSlot First = new(0);
    public static readonly BrawlerActionSlot Second = new(1);
    public static readonly BrawlerActionSlot Third = new(2);

    public BrawlerActionSlot Next(BrawlerActionCategory category) {
        return Get((Value + 1) % category.Value);
    }

    public static bool operator <(BrawlerActionSlot left, BrawlerActionSlot right) {
        return left.Value < right.Value;
    }

    public static bool operator >(BrawlerActionSlot left, BrawlerActionSlot right) {
        return left.Value > right.Value;
    }

    public static bool operator <=(BrawlerActionSlot left, BrawlerActionSlot right) {
        return left.Value <= right.Value;
    }

    public static bool operator >=(BrawlerActionSlot left, BrawlerActionSlot right) {
        return left.Value >= right.Value;
    }

    public static bool operator <(BrawlerActionSlot left, BrawlerActionCategory right) {
        return left.Value < right.Value;
    }

    public static bool operator >(BrawlerActionSlot left, BrawlerActionCategory right) {
        return left.Value > right.Value;
    }

    public static bool operator <=(BrawlerActionSlot left, BrawlerActionCategory right) {
        return left.Value <= right.Value;
    }

    public static bool operator >=(BrawlerActionSlot left, BrawlerActionCategory right) {
        return left.Value >= right.Value;
    }

    public static bool operator ==(int left, BrawlerActionSlot right) {
        return left == right.Value;
    }

    public static bool operator !=(int left, BrawlerActionSlot right) {
        return left != right.Value;
    }

    public static bool operator ==(BrawlerActionSlot left, int right) {
        return left.Value == right;
    }

    public static bool operator !=(BrawlerActionSlot left, int right) {
        return left.Value != right;
    }

    public static BrawlerActionSlot Get(int value) {
        return value switch {
            0 => First,
            1 => Second,
            2 => Third,
            _ => new BrawlerActionSlot(value)
        };
    }
}