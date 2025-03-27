#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using brawlstars.Brawlstars.Common.Systems;
using brawlstars.Brawlstars.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace brawlstars.Brawlstars;

/// <summary>
/// 每个英雄都从此类派生出相关武器的物品。
/// 应每个命名空间仅有一个此类的子类且命名为"Item"。
/// </summary>
public abstract class BrawlerItem : ModItem {
    /// <summary>
    /// 行为。
    /// </summary>
    public required BrawlerBehavior Behavior;

    /// <summary>
    /// 英雄名称。
    /// 全部大写。
    /// </summary>
    public abstract string BrawlerName { get; }

    public abstract Rarity Rarity { get; }

    public override string LocalizationCategory => "Brawlers." + BrawlerName;
    public override string Name => BrawlerName + "_Item";
    public override string Texture => (GetType().Namespace + "/Texture/Item").Replace('.', '/');

    public string BrawlerDesc => Behavior.BrawlerInfo.BrawlerDescription;
    public HeroInfo[] HeroInfos => Behavior.BrawlerInfo.HeroInfos;
    public GadgetInfo[] GadgetInfos => Behavior.BrawlerInfo.GadgetInfos;
    public StarPowerInfo[] StarPowerInfos => Behavior.BrawlerInfo.StarPowerInfos;

    public override void SetDefaults() {
        Item.useStyle = ItemUseStyleID.Thrust;
        // 行为逻辑与动画刻度耦合。
        // 所有操作都应该在至多24帧内完成。
        Item.useAnimation = 24;
        Item.useTime = 24;
        Item.noMelee = true;
        Item.value = 8900000;
        Item.rare = ItemRarityID.Expert;
    }

    public override void LoadData(TagCompound tag) {
        Behavior.GadgetSlot = tag.GetInt("GadgetSlot");
        Behavior.StarPowerSlot = tag.GetInt("StarPowerSlot");

        if (Behavior.GadgetSlot < 0 || Behavior.GadgetSlot >= Behavior.GadgetCategory) {
            Behavior.GadgetSlot = 0;
        }

        if (Behavior.StarPowerSlot < 0 || Behavior.StarPowerSlot >= Behavior.StarPowerCategory) {
            Behavior.StarPowerSlot = 0;
        }
    }

    public override void SaveData(TagCompound tag) {
        tag.Set("GadgetSlot", Behavior.GadgetSlot);
        tag.Set("StarPowerSlot", Behavior.StarPowerSlot);
    }

    public override void NetSend(BinaryWriter writer) {
        writer.Write(Behavior.GadgetSlot);
        writer.Write(Behavior.StarPowerSlot);
    }

    public override void NetReceive(BinaryReader reader) {
        Behavior.GadgetSlot = reader.ReadInt32();
        Behavior.StarPowerSlot = reader.ReadInt32();
    }

    public override bool AltFunctionUse(Player player) {
        return true;
    }


    private string NotImplemented() {
        return
            TooltipInfo.ItemTooltip.NotImplementedColor2.ColorText(" {{") +
            TooltipInfo.ItemTooltip.NotImplementedColor1.ColorText(
                TooltipInfo.ItemTooltip.NotImplemented
            ) +
            TooltipInfo.ItemTooltip.NotImplementedColor2.ColorText("}}");
    }


    public override void ModifyTooltips(List<TooltipLine> tooltips) {
        tooltips.Tooltip(
            "Rarity",
            Rarity.Color.ColorText(Rarity.LocalizedText)
        );

        if (Behavior.NotImplementedAll) {
            tooltips.Tooltip("BrawlerDesc", NotImplemented());
            return;
        }

        tooltips.Tooltip(
            "BrawlerDesc",
            TooltipInfo.ItemTooltip.DescriptionColor.ColorText(BrawlerDesc)
        );

        for (var i = 0; i < Behavior.HeroCategory; i++) {
            var traits = Behavior.BrawlerInfo.HeroInfos[i].Traits;
            var attack = HeroInfos[i].Attack.Name;
            var super = HeroInfos[i].Super.Name;
            var hyperCharge = HeroInfos[i].HyperCharge.Name;

            if (traits.Length != 0 || attack.Length != 0 || super.Length != 0 || hyperCharge.Length != 0) {
                tooltips.Tooltip(
                    $"HeroStats{i}",
                    TooltipInfo.ItemTooltip.HeroStatsColor.ColorText(TooltipInfo.ItemTooltip.HeroStats)
                );
            } else {
                continue;
            }

            var localI = i;

            traits.ForEachIndexed((index, trait) => {
                if (trait.Trait.Length != 0) {
                    tooltips.Tooltip(
                        $"Hero{localI}Traits{index}",
                        $"{TooltipInfo.ItemTooltip.TraitsColor.ColorText(TooltipInfo.ItemTooltip.Traits)}: {trait.Trait}{(Behavior.NotImplementedTraits[localI] ? NotImplemented() : "")}"
                    );
                }
            });


            if (attack.Length != 0) {
                tooltips.Tooltip(
                    $"Hero{i}Attack",
                    $"{TooltipInfo.ItemTooltip.AttackColor.ColorText(TooltipInfo.ItemTooltip.Attack)}: {attack}{(Behavior.NotImplementedAttack[i] ? NotImplemented() : "")}"
                );
                tooltips.Tooltip(
                    $"Hero{i}AttackDesc",
                    HeroInfos[i].Attack.DescriptionTooltip(
                        TooltipInfo.ItemTooltip.DescriptionColor,
                        TooltipInfo.ItemTooltip.DescriptionValueColor
                    )
                );
                tooltips.Tooltip(
                    $"Hero{i}AttackStat",
                    HeroInfos[i].Attack.StatsTooltip(TooltipInfo.ItemTooltip.AttackStatKeyColor)
                );
            }

            if (super.Length != 0) {
                tooltips.Tooltip(
                    $"Hero{i}Super",
                    $"{TooltipInfo.ItemTooltip.SuperColor.ColorText(TooltipInfo.ItemTooltip.Super)}: {super}{(Behavior.NotImplementedSuper[i] ? NotImplemented() : "")}"
                );
                tooltips.Tooltip(
                    $"Hero{i}SuperDesc",
                    HeroInfos[i].Super.DescriptionTooltip(
                        TooltipInfo.ItemTooltip.DescriptionColor,
                        TooltipInfo.ItemTooltip.DescriptionValueColor
                    )
                );
                tooltips.Tooltip(
                    $"Hero{i}SuperStat",
                    HeroInfos[i].Super.StatsTooltip(TooltipInfo.ItemTooltip.SuperStatKeyColor)
                );
            }

            if (hyperCharge.Length != 0) {
                tooltips.Tooltip(
                    $"Hero{i}HyperCharge",
                    $"{TooltipInfo.ItemTooltip.HyperChargeColor.ColorText(TooltipInfo.ItemTooltip.HyperCharge)}: {HeroInfos[i].HyperCharge.Name}{(Behavior.NotImplementedHyperCharge[i] ? NotImplemented() : "")}{KeyBinds.HyperChargeUseTooltip()}"
                );
                tooltips.Tooltip(
                    $"Hero{i}HyperChargeDesc",
                    HeroInfos[i].HyperCharge.DescriptionTooltip(
                        TooltipInfo.ItemTooltip.DescriptionColor,
                        TooltipInfo.ItemTooltip.DescriptionValueColor
                    )
                );
                tooltips.Tooltip(
                    $"Hero{i}HyperChargeStat",
                    string.Join(" ",
                        HeroInfos[i].HyperCharge.StatsTooltip(TooltipInfo.ItemTooltip.HyperChargeStatKeyColor)
                    )
                );
            }
        }

        for (var i = 0; i < Behavior.GadgetCategory; i++) {
            if (GadgetInfos[i].Name.Length != 0) {
                tooltips.Tooltip(
                    $"Gadget{i}",
                    $"{TooltipInfo.ItemTooltip.GadgetColor.ColorText(TooltipInfo.ItemTooltip.Gadget)}: {GadgetInfos[i].Name}{(Behavior.NotImplementedGadget[i] ? NotImplemented() : "")}" +
                    $"{(Behavior.GadgetSlot == i ? KeyBinds.GadgetUseTooltip() : KeyBinds.GadgetSelectTooltip())}"
                );
                tooltips.Tooltip(
                    $"Gadget{i}Desc",
                    GadgetInfos[i].DescriptionTooltip(
                        TooltipInfo.ItemTooltip.DescriptionColor,
                        TooltipInfo.ItemTooltip.DescriptionValueColor
                    )
                );
            }
        }

        for (var i = 0; i < Behavior.StarPowerCategory; i++) {
            if (StarPowerInfos[i].Name.Length != 0) {
                tooltips.Tooltip(
                    $"StarPower{i}",
                    $"{TooltipInfo.ItemTooltip.StarPowerColor.ColorText(TooltipInfo.ItemTooltip.StarPower)}: {StarPowerInfos[i].Name}{(Behavior.NotImplementedStarPower[i] ? NotImplemented() : "")}" +
                    $"{(Behavior.StarPowerSlot == i ? KeyBinds.StarPowerSelectedTooltip() : KeyBinds.StarPowerSelectTooltip())}"
                );
                tooltips.Tooltip(
                    $"StarPower{i}Desc",
                    StarPowerInfos[i].DescriptionTooltip(
                        TooltipInfo.ItemTooltip.DescriptionColor,
                        TooltipInfo.ItemTooltip.DescriptionValueColor
                    )
                );
            }
        }
    }

    public override void ModifyItemScale(Player player, ref float scale) {
        base.ModifyItemScale(player, ref scale);
        scale = 0;
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.LunarBar, 20)
            .AddIngredient(ItemID.Emerald, 999)
            .Register();
    }


    protected string L(string suffix) {
        return this.GetLocalization(suffix, () => "").Value;
    }
}