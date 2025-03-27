using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class StaticAction2 : BrawlerAction<Behavior> {
    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        var direction = Player.direction;
        var drawData = Behavior.BuildDrawData(direction);
        if (Player.PlayerPoseIsMove()) {
        }else if (Player.PlayerPoseIsHover()) {
            drawData.rotation += -direction * 1.45f;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}