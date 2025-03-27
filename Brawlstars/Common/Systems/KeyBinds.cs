using brawlstars.Brawlstars.Utils;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Common.Systems;

public class KeyBinds : ModSystem {
    public static ModKeybind GadgetSelectKeybind { get; private set; }
    public static ModKeybind StarPowerSelectKeybind { get; private set; }
    public static ModKeybind GadgetUseKeybind { get; private set; }
    public static ModKeybind HyperChargeUseKeybind { get; private set; }

    public static string GadgetSelectTooltip() {
        return "  " +
               TooltipInfo.ItemTooltip.SelectColor.ColorText(TooltipInfo.ItemTooltip.Select) +
               $": [{string.Join(", ", GadgetSelectKeybind.GetAssignedKeys())}]";
    }


    public static string StarPowerSelectTooltip() {
        return "  " +
               TooltipInfo.ItemTooltip.SelectColor.ColorText(TooltipInfo.ItemTooltip.Select) +
               $": [{string.Join(", ", StarPowerSelectKeybind.GetAssignedKeys())}]";
    }

    public static string StarPowerSelectedTooltip() {
        return "  " +
               TooltipInfo.ItemTooltip.SelectedColor.ColorText(TooltipInfo.ItemTooltip.Selected);
    }


    public static string GadgetUseTooltip() {
        return "  " +
               TooltipInfo.ItemTooltip.UseColor.ColorText(TooltipInfo.ItemTooltip.Use) +
               $": [{string.Join(", ", GadgetUseKeybind.GetAssignedKeys())}]";
    }

    public static string HyperChargeUseTooltip() {
        return "  " +
               TooltipInfo.ItemTooltip.UseColor.ColorText(TooltipInfo.ItemTooltip.Use) +
               $": [{string.Join(", ", HyperChargeUseKeybind.GetAssignedKeys())}]";
    }

    public override void Load() {
        GadgetSelectKeybind = KeybindLoader.RegisterKeybind(BrawlStars.Instance, "GadgetSelect", "NumPad1");
        StarPowerSelectKeybind = KeybindLoader.RegisterKeybind(BrawlStars.Instance, "StarPowerSelect", "NumPad2");
        GadgetUseKeybind = KeybindLoader.RegisterKeybind(BrawlStars.Instance, "GadgetUse", "Q");
        HyperChargeUseKeybind = KeybindLoader.RegisterKeybind(BrawlStars.Instance, "HyperChargeUse", "E");
    }

    public override void Unload() {
        GadgetSelectKeybind = null;
        StarPowerSelectKeybind = null;
        GadgetUseKeybind = null;
        HyperChargeUseKeybind = null;
    }
}