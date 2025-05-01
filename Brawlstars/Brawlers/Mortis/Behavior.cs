using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class Behavior : BrawlerBehavior {
    public Behavior(BrawlerItem item) : base(item) {
    }

    protected override void BuildInfo(BrawlerInfo.BrawlerInfoBuilder builder) {
        builder
            .Hero(
                0,
                [],
                [[2000], [4.5]],
                [],
                [[1800], ["12.5%"]],
                [],
                [24, 25, 5]
            )
            .Gadget([2000])
            .Gadget([4])
            .StarPower([100])
            .StarPower([2]);
    }

    protected override void InitHyperCharge() {
        base.InitHyperCharge();
        HyperChargeMultiplier[HeroSlot] = 40;
    }

    protected override void InitAllNotImplementationMarked() {
        base.InitAllNotImplementationMarked();
        NotImplementedHyperCharge = [true];
        NotImplementedStarPower = [false, true];
    }

    public readonly StaticAction StaticAction = null;
    public readonly AttackAction AttackAction = null;
    public readonly SuperAction SuperAction = null;
    public readonly GadgetAction1 GadgetAction1 = null;
    public readonly GadgetAction2 GadgetAction2 = null;
    public readonly StarPowerAction1 StarPowerAction1 = null;

    private Texture2D ShovelRight => RequestTexture("Shovel_Right");
    private Texture2D ShovelLeft => RequestTexture("Shovel_Left");

    private Texture2D ShovelRightGadget2 => RequestTexture("Shovel_Right_Gadget2");
    private Texture2D ShovelLeftGadget2 => RequestTexture("Shovel_Left_Gadget2");

    private Texture2D ShovelTexture(bool isRight, bool isHasLeftTime) {
        return isRight ? isHasLeftTime ? ShovelRightGadget2 : ShovelRight :
            isHasLeftTime ? ShovelLeftGadget2 : ShovelLeft;
    }

    private Texture2D ShovelTextureFromDirection(int direction) {
        return ShovelTexture(direction == 1, GadgetView.HasCooldown);
    }

    public DrawData BuildDrawData(int direction) {
        var drawData = Player.BuildPlayerCenterDrawData(ShovelTextureFromDirection(direction));
        drawData.destinationRectangle.FrontPlus(direction, Player.PlayerDrawHandOffset());
        drawData.origin = drawData.texture.TextureCenter().PlusX(-direction * 4);
        return drawData;
    }
}