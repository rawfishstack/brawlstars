using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class GadgetAction2 : BrawlerAction<Behavior> {
    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        Behavior.StaticAction.PostAction(ref drawInfo);
        if (!Behavior.GadgetUsed) {
            if (Player.itemAnimation == 24) Player.itemAnimation = 6;
            return;
        }

        Behavior.GadgetView.SetCooldownToMax();
        drawInfo.drawPlayer.itemAnimation = 1;
    }

    protected override void OnEnable() {
        Behavior.GadgetView.MaxCooldown = 240;
    }

    protected override void OnDisable() {
        Behavior.GadgetView.Cooldown = 0;
        Behavior.GadgetView.MaxCooldown = 0;
    }
}