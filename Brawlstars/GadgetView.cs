using Microsoft.Xna.Framework;

namespace brawlstars.Brawlstars;

public class GadgetView(BrawlerBehavior behavior) : ChargingView(behavior) {
    private readonly Color _color = new(117, 255, 122);
    private readonly Color _fillColor = new(43, 237, 49);
    private int _progress;

    public int Progress {
        get => _progress;
        set {
            _progress = value;
            if (_progress < 0) _progress = 0;
            if (_progress > 300) _progress = 300;
        }
    }


    public override int Value => Progress / 3;

    public override Color ThemeColor => Value >= 100 ? _fillColor : _color;
    public override string Tip => Behavior.BrawlerInfo.GadgetInfos[Behavior.GadgetSlot].Name;
    public override string TipColor => TooltipInfo.ItemTooltip.GadgetColor;

    public override void Reset() {
        Progress = 0;
    }

    protected override void ActualUpdate() {
        Progress++;
    }

    protected override bool ActualNext() {
        if (Progress == 300) {
            Progress = 0;
            return true;
        }

        return false;
    }
}