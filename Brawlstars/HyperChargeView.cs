using System;
using Microsoft.Xna.Framework;

namespace brawlstars.Brawlstars;

public class HyperChargeView(BrawlerBehavior behavior) : ChargingView(behavior) {
    private float _charge;

    public float Charge {
        get => _charge;
        set {
            _charge = value;

            if (_charge >= 100) {
                _charge = 100;
            }

            if (_charge < 0) _charge = 0;
        }
    }

    public override int Value => (int)Charge;
    public override Color ThemeColor { get; } = new(255, 84, 249);
    public override string Tip => Behavior.BrawlerInfo.HeroInfos[Behavior.HeroSlot].HyperCharge.Name;
    public override string TipColor => TooltipInfo.ItemTooltip.HyperChargeColor;

    public override void Reset() {
    }

    protected override void ActualUpdate() {
    }

    protected override bool ActualNext() {
        return false;
    }
}