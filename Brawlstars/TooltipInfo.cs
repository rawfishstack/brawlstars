using System;

namespace brawlstars.Brawlstars;

public static class TooltipInfo {
    private static readonly BrawlStars Mod = BrawlStars.Instance;
    private static readonly string Name = nameof(TooltipInfo) + ".";

    private static Func<string, string> Localization(string name) {
        return arg => { return Mod.GetLocalization(name + arg, () => "").Value; };
    }


    public static class ItemTooltip {
        public static readonly string Name = TooltipInfo.Name + nameof(ItemTooltip) + ".";
        public static readonly Func<string, string> L = Localization(Name);

        public static string HeroStatsColor = "F4E6FF";
        public static string TraitsColor = "ABBCFF";
        public static string AttackColor = "FF4545";
        public static string SuperColor = "FFF130";
        public static string GadgetColor = "8CFF69";
        public static string StarPowerColor = "FF8336";
        public static string HyperChargeColor = "FF45F9";
        public static string DescriptionColor = "9A9A9A";
        public static string AttackStatKeyColor = "FFD8B8";
        public static string SuperStatKeyColor = "FFFEB8";
        public static string HyperChargeStatKeyColor = "F7C2FF";
        public static string AttackStatValueColor = "FFFFFF";
        public static string SuperStatValueColor = "FFFFFF";
        public static string HyperChargeStatValueColor = "FFFFFF";
        public static string DescriptionValueColor = "8FFF8F";
        public static string SelectColor = "FFFEC9";
        public static string UseColor = "FFD1C9";
        public static string SelectedColor = "FFD1C9";
        public static string NotImplementedColor1 = "FCCA03";
        public static string NotImplementedColor2 = "FF0019";

        public static string Rarity => L(nameof(Rarity));
        public static string HeroStats => L(nameof(HeroStats));
        public static string Traits => L(nameof(Traits));
        public static string Attack => L(nameof(Attack));
        public static string Super => L(nameof(Super));
        public static string Gadget => L(nameof(Gadget));
        public static string StarPower => L(nameof(StarPower));
        public static string HyperCharge => L(nameof(HyperCharge));
        public static string Select => L(nameof(Select));
        public static string Use => L(nameof(Use));
        public static string Selected => L(nameof(Selected));
        public static string NotImplemented => L(nameof(NotImplemented));
    }


    public static class Rarities {
        private static readonly string Name = TooltipInfo.Name + nameof(Rarities) + ".";
        private static readonly Func<string, string> L = Localization(Name);

        public static string Starting => L(nameof(Starting));
        public static string Rare => L(nameof(Rare));
        public static string SuperRare => L(nameof(SuperRare));
        public static string Epic => L(nameof(Epic));
        public static string Mythic => L(nameof(Mythic));
        public static string Legendary => L(nameof(Legendary));
    }
}