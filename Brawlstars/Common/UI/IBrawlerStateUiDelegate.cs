using Microsoft.Xna.Framework.Graphics;

namespace brawlstars.Brawlstars.Common.UI;

public interface IBrawlerStateUiDelegate {
    public void Show(BrawlerStateUiContainer ui);

    public  void BeforeDraw(BrawlerStateUiContainer ui, SpriteBatch spriteBatch);

    public void Hide(BrawlerStateUiContainer ui);
}