using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class StaticAction1 : BrawlerAction<Behavior> {
    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        var direction = Player.direction;
        var drawData = Behavior.BuildDrawData(direction);
        if (Player.PlayerPoseIsMove()) {
        }else if (Player.PlayerPoseIsHover()) {
            drawData.rotation += -direction * 1.45f;
        } else {
            drawData.rotation += direction * 0.5f * float.Pi;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}