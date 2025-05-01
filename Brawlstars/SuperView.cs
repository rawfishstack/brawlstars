#nullable enable
using Microsoft.Xna.Framework;

namespace brawlstars.Brawlstars;

public class SuperView(BrawlerBehavior behavior) : ChargingView(behavior) {
    private readonly Color _color = new(255, 215, 97);
    private readonly Color _fillColor = new(255, 155, 61);

    private float _charge;

    private int _count;

    /**
     * 充能满时获得的大招数量。
     */
    public int MaxCount = 1;

    public int Count {
        get => _count;
        set {
            _count = value;
            if (_count > MaxCount) _count = MaxCount;
            if (_count < 0) _count = 0;
        }
    }

    public float Charge {
        get => _charge;
        set {
            _charge = value;

            if (_charge >= 100) {
                _charge = 100;
                if (Count == 0) Count = MaxCount;
            }

            if (_charge < 0) _charge = 0;
        }
    }


    public override int Value => (int)Charge;

    public override Color ThemeColor => Value >= 100 ? _fillColor : _color;

    public override string Tip => Behavior.BrawlerInfo
        .HeroInfos[Behavior.BrawlerActionSlots[BrawlerActionAggregate.Hero].Value].Super.Name;

    public override string TipColor => TooltipInfo.ItemTooltip.SuperColor;

    public override void Reset() {
        Charge = 0;
    }

    protected override void ActualUpdate() {
    }

    protected override bool ActualNext() {
        return Count-- > 0 && Count == 0;
    }
}