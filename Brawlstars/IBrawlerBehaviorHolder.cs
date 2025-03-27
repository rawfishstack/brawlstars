using Terraria;

namespace brawlstars.Brawlstars;

public interface IBrawlerBehaviorHolder<out T> where T : BrawlerBehavior {
    public T Behavior { get; }

    public void SetBehavior(BrawlerBehavior behavior);
}