using Terraria;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars;

public class BrawlerAction<T> : IBrawlerAction<T> where T : BrawlerBehavior {
    public T Behavior { get; private set; }

    public void SetBehavior(BrawlerBehavior behavior) {
        Behavior = (T)behavior;
    }
    
    public Player Player => Behavior.Player;

    public void PostAction(ref PlayerDrawSet drawInfo) {
        OnAction(ref drawInfo);
    }

    public void PostEnable() {
        OnEnable();
    }

    public void PostDisable() {
        OnDisable();
    }

    // todo 将动作分离出过程列表，进行更精细的控制
    protected virtual void OnAction(ref PlayerDrawSet drawInfo) {
    }

    protected virtual void OnEnable() {
    }

    protected virtual void OnDisable() {
    }
}