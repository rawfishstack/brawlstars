namespace brawlstars.Brawlstars.Brawlers.Meg;

public class GadgetAction1 : BrawlerAction<Behavior> {
    protected override void OnEnable() {
        Behavior.GadgetView.Usable = Behavior.HeroSlot == 1;
    }
}