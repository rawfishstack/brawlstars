#nullable enable
using System.Collections.Generic;
using Terraria.UI;

namespace brawlstars.Brawlstars.Common.UI;

public sealed class BrawlerStateGroupUi : UIState {
    private readonly List<BrawlerStateUiContainer> _containers = [
        new(0, 0, behavior => behavior.HyperChargeView),
        new(1, 0, behavior => behavior.SuperView),
        new(2, 0, behavior => behavior.GadgetView),
        new(3, 0, behavior => behavior.ExtraView)
    ];

    public override void OnInitialize() {
        _containers.ForEach(Append);
    }

    public void Show(BrawlerItem brawlerItem) {
        _containers.ForEach(ui => ui.Show(brawlerItem.Behavior));
    }

    public void Hide() {
        _containers.ForEach(ui => ui.Hide());
    }
}