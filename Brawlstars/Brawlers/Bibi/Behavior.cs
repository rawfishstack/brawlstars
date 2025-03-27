using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Bibi;

public class Behavior : BrawlerBehavior {
    public Behavior(BrawlerItem item) : base(item) {
    }

    protected override void BuildInfo(BrawlerInfo.BrawlerInfoBuilder builder) {
        builder
            .Hero(
                0,
                [],
                [[2800]],
                [],
                [[1800]],
                [],
                [24, 5, 25]
            )
            .Gadget([50, 4])
            .Gadget([3])
            .StarPower(["12%"])
            .StarPower(["20%"]);
    }

    protected override void InitHyperCharge() {
        base.InitHyperCharge();
        HyperChargeMultiplier[0] = 50;
    }

    protected override void InitAllNotImplementationMarked() {
        base.InitAllNotImplementationMarked();
        NotImplementedHyperCharge = [true];
        NotImplementedGadget = [false, true];
        NotImplementedStarPower = [true, true];
    }

    public readonly StaticAction StaticAction = null;
    public readonly AttackAction AttackAction = null;
    public readonly SuperAction SuperAction = null;
    public readonly GadgetAction1 GadgetAction1 = null;

    private Texture2D BaseballBatRight => RequestTexture("Baseball_Bat_Right");
    private Texture2D BaseballBatLeft => RequestTexture("Baseball_Bat_Left");

    private Texture2D BaseballBatTextureFromDirection(int direction) {
        return direction == 1 ? BaseballBatRight : BaseballBatLeft;
    }

    public DrawData BuildDrawData(int direction) {
        var drawData = Player.BuildPlayerCenterDrawData(BaseballBatTextureFromDirection(direction));
        drawData.destinationRectangle.FrontPlus(direction, Player.PlayerDrawHandOffset());
        drawData.origin = drawData.texture.TextureCenter().PlusX(-direction * 12);
        return drawData;
    }

    protected override void OnDraw(ref PlayerDrawSet drawInfo) {
        base.OnDraw(ref drawInfo);
        if (GadgetView is { HasCooldown: true, Cooldown: 1 or 61 or 121 or 181 }) {
            Player.Heal(50);
        }
    }
}