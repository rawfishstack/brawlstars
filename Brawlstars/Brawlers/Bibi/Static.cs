using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Bibi;

public class StaticAction : BrawlerAction<Behavior> {
    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        var direction = Player.direction;
        var drawData = Behavior.BuildDrawData(direction);
        if (Player.PlayerPoseIsMove()) {
            drawData.rotation += -direction * 0.4f;
        } else if (Player.PlayerPoseIsHover()) {
            drawData.rotation += -direction * 1.3f;
        } else {
            drawData.rotation += direction * 0.2f;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}