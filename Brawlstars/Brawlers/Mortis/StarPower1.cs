namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class StarPowerAction1 : BrawlerAction<Behavior> {
    public void Heal() {
        if (Behavior.BrawlerActionSlots[BrawlerActionAggregate.StarPower] == 0) {
            Player.Heal(100);
        }
    }
}