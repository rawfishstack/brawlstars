using System;

namespace brawlstars.Brawlstars;

public class Rarity {
    public static Rarity Starting = new("B9EAFF", () => TooltipInfo.Rarities.Starting);
    public static Rarity Rare = new("68FD58", () => TooltipInfo.Rarities.Rare);
    public static Rarity SuperRare = new("5AB3FF", () => TooltipInfo.Rarities.SuperRare);
    public static Rarity Epic = new("D850FF", () => TooltipInfo.Rarities.Epic);
    public static Rarity Mythic = new("FE5E72", () => TooltipInfo.Rarities.Mythic);
    public static Rarity Legendary = new("FFF11E", () => TooltipInfo.Rarities.Legendary);

    private readonly Func<string> _localizedText;

    public Rarity(string color, Func<string> localizedText) {
        Color = color;
        _localizedText = localizedText;
    }

    public string Color { get; }

    public string LocalizedText => _localizedText();
}