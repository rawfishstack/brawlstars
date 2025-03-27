using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace brawlstars.Brawlstars.Common.UI;

public class BrawlerStateUiContainer(
    int xNegativeLocation,
    int yLocation,
    Func<BrawlerBehavior, IBrawlerStateUiDelegate> delegateProvider
) : UIElement {
    private IBrawlerStateUiDelegate? _delegate;

    public override void OnInitialize() {
        Width.Set(112, 0);
        Height.Set(53, 0);
        Top.Set(16 + yLocation * 55, 0);
        Left.Set(-420 - xNegativeLocation * 119, 1);
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        _delegate?.BeforeDraw(this, spriteBatch);
        base.DrawSelf(spriteBatch);
    }

    public void Show(BrawlerBehavior behavior) {
        _delegate = delegateProvider(behavior);
        _delegate?.Show(this);
    }

    public void Hide() {
        _delegate?.Hide(this);
        _delegate = null;
    }
}