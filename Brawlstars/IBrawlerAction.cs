using Terraria.DataStructures;

namespace brawlstars.Brawlstars;

public interface IBrawlerAction<out T> : IBrawlerBehaviorHolder<T> where T : BrawlerBehavior {
    public void PostAction(ref PlayerDrawSet drawInfo);
    public void PostEnable();
    public void PostDisable();
}