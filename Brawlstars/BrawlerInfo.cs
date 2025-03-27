using System;
using System.Collections.Generic;
using brawlstars.Brawlstars.Utils;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public class BrawlerInfo(Func<Mod> mod, string brawlerName, string localizationCategory) : ILocalizedModType {
    public Mod Mod => mod();
    public string Name => brawlerName + "_Info";
    public string FullName => $"brawlstars/{Name}";
    public string LocalizationCategory => localizationCategory;


    public string BrawlerDescription => this.GetLocalization(nameof(BrawlerDescription), () => "").Value;


    public HeroInfo[] HeroInfos { get; private set; }
    public GadgetInfo[] GadgetInfos { get; private set; }
    public StarPowerInfo[] StarPowerInfos { get; private set; }


    public sealed class BrawlerInfoBuilder(Func<Mod> mod, string brawlerName, string localizationCategory) {
        private record HeroMeta(
            int TraitCategory,
            object[] AttackDescriptionValues,
            object[][] AttackStatValues,
            object[] SuperDescriptionValues,
            object[][] SuperStatValues,
            object[] HyperChargeDescriptionValues,
            int[] HyperChargeStatValues);

        private record GadgetMeta(object[] Values);

        private record StarPowerMeta(object[] Values);

        private readonly List<HeroMeta> _heroMetas = [];
        private readonly List<GadgetMeta> _gadgetMetas = [];
        private readonly List<StarPowerMeta> _starPowerMetas = [];

        public BrawlerInfoBuilder Hero(
            int traitCategory,
            object[] attackDescriptionValues,
            object[][] attackStatValues,
            object[] superDescriptionValues,
            object[][] superStatValues,
            object[] hyperChargeDescriptionValues,
            int[] hyperChargeStatValues
        ) {
            _heroMetas.Add(new HeroMeta(
                traitCategory,
                attackDescriptionValues,
                attackStatValues,
                superDescriptionValues,
                superStatValues,
                hyperChargeDescriptionValues,
                hyperChargeStatValues
            ));
            return this;
        }

        public BrawlerInfoBuilder Gadget(object[] values) {
            _gadgetMetas.Add(new GadgetMeta(values));
            return this;
        }

        public BrawlerInfoBuilder StarPower(object[] values) {
            _starPowerMetas.Add(new StarPowerMeta(values));
            return this;
        }

        public BrawlerInfo Build() {
            var info = new BrawlerInfo(mod, brawlerName, localizationCategory);
            info.HeroInfos = _heroMetas.Count.Array(
                i0 => new HeroInfo(
                    info, i0.ToOneBased(), _heroMetas[i0].TraitCategory,
                    _heroMetas[i0].AttackDescriptionValues,
                    _heroMetas[i0].AttackStatValues,
                    _heroMetas[i0].SuperDescriptionValues,
                    _heroMetas[i0].SuperStatValues,
                    _heroMetas[i0].HyperChargeDescriptionValues,
                    _heroMetas[i0].HyperChargeStatValues
                )
            );
            info.GadgetInfos = _gadgetMetas.Count.Array(
                i0 => new GadgetInfo(info, i0.ToOneBased(), _gadgetMetas[i0].Values)
            );
            info.StarPowerInfos = _starPowerMetas.Count.Array(
                i0 => new StarPowerInfo(info, i0.ToOneBased(), _starPowerMetas[i0].Values)
            );
            return info;
        }
    }
}

public class InfoStruct(string prefix, BrawlerInfo info) {
    public readonly string Prefix = prefix;

    protected string L(string suffix) {
        return info.GetLocalization($"{Prefix}.{suffix}", () => string.Empty).Value;
    }

    protected string L() {
        return info.GetLocalization(Prefix, () => string.Empty).Value;
    }
}

public class NdInfo : InfoStruct {
    public NdInfo(string prefix, BrawlerInfo info, object[] descriptionValues) : base(prefix, info) {
        DescriptionValues = descriptionValues.Map(o => o.ToString());
    }

    public string Name => L(nameof(Name));
    public string Description => L(nameof(Description));

    public string[] DescriptionValues;

    public string DescriptionTooltip(string descriptionColor, string valueColor) {
        return descriptionColor.ColorText(string.Format(Description,
            DescriptionValues.Map(object (v) => valueColor.ColorTextInsert(v, descriptionColor))
        ));
    }
}

public class NdsInfo : InfoStruct {
    public NdsInfo(string prefix, BrawlerInfo info, object[] descriptionValues, object[][] statValues) :
        base(prefix, info) {
        DescriptionValues = descriptionValues.Map(o => o.ToString());
        Stats = statValues.MapIndexed((i0, values) => new StatInfo(Prefix, info, i0.ToOneBased(), values));
    }

    public string Name => L(nameof(Name));
    public string Description => L(nameof(Description));
    public string[] DescriptionValues;
    public StatInfo[] Stats;


    public string DescriptionTooltip(string descriptionColor, string valueColor) {
        return descriptionColor.ColorText(string.Format(Description,
            DescriptionValues.Map(object (v) => valueColor.ColorTextInsert(v, descriptionColor))
        ));
    }

    public string StatsTooltip(string keyColor) {
        return string.Join(" ",
            Stats.Map(
                stat => stat.Tooltip(keyColor)
            )
        );
    }
}

public class StatInfo(string parent, BrawlerInfo info, int i1, object[] values)
    : InfoStruct($"{parent}.Stats.{i1}", info) {
    public string Key => L("Key");
    public string Value => string.Format(L("Value"), values);

    public string Tooltip(string keyColor) {
        return keyColor.ColorText(Key) + ": " + Value;
    }
}

public class HeroInfo : InfoStruct {
    public HeroInfo(
        BrawlerInfo info, int i1, int traitCategory,
        object[] attackDescriptionValues,
        object[][] attackStatValues,
        object[] superDescriptionValues,
        object[][] superStatValues,
        object[] hyperChargeDescriptionValues,
        int[] hyperChargeStatValues
    ) : base($"Heroes.{i1}", info) {
        Traits = traitCategory.Array(i0 => new TraitsInfo(Prefix, info, i0.ToOneBased()));
        Attack = new NdsInfo($"{Prefix}.Attack", info, attackDescriptionValues, attackStatValues);
        Super = new NdsInfo($"{Prefix}.Super", info, superDescriptionValues, superStatValues);
        HyperCharge = new NdsInfo($"{Prefix}.HyperCharge", info, hyperChargeDescriptionValues,
            hyperChargeStatValues.Map(
                object[] (v) => [v.ToString()]
            )
        );
    }

    public readonly TraitsInfo[] Traits;
    public readonly NdsInfo Attack;
    public readonly NdsInfo Super;
    public readonly NdsInfo HyperCharge;
}

public class TraitsInfo(string parent, BrawlerInfo info, int i1) : InfoStruct($"{parent}.Traits.{i1}", info) {
    public string Trait => L();
}

public class GadgetInfo(BrawlerInfo info, int i1, object[] descriptionValues)
    : NdInfo($"Gadgets.{i1}", info, descriptionValues);

public class StarPowerInfo(BrawlerInfo info, int i1, object[] descriptionValues)
    : NdInfo($"StarPowers.{i1}", info, descriptionValues);