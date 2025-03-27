using brawlstars.Brawlstars.Utils;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class StaticAction : BrawlerAction<Behavior> {
    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        var direction = Player.direction;
        var drawData = Behavior.BuildDrawData(direction);
        drawData.origin = drawData.origin.PlusX(-direction * 2);
        if (Player.PlayerPoseIsMove()) {
            drawData.rotation += -direction * 2f;
        }

        if (Player.PlayerPoseIsHover()) {
            drawData.rotation += -direction * 1.3f;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}