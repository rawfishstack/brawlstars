using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class Behavior : BrawlerBehavior {
    public Behavior(BrawlerItem item) : base(item) {
    }

    protected override void BuildInfo(BrawlerInfo.BrawlerInfoBuilder builder) {
        builder
            .Hero(
                0,
                [],
                [[2, 600]],
                [],
                [[0.4]],
                [],
                [0, 0, 0]
            )
            .Hero(
                0,
                [],
                [[16, 300]],
                [],
                [[3400]],
                [],
                [0, 0, 0]
            )
            .Gadget([35, 5])
            .Gadget(["35%"])
            .StarPower(["25%", 10])
            .StarPower([2400]);
    }

    protected override void InitAllNotImplementationMarked() {
        base.InitAllNotImplementationMarked();
        NotImplementedGadget = [true, true];
        NotImplementedStarPower = [true, true];
    }

    public readonly StaticAction1 StaticAction1 = null;
    public readonly StaticAction2 StaticAction2 = null;
    public readonly AttackAction1 AttackAction1 = null;
    public readonly AttackAction2 AttackAction2 = null;
    public readonly SuperAction1 SuperAction1 = null;
    public readonly SuperAction2 SuperAction2 = null;

    private Texture2D Weapon1Right => RequestTexture("Weapon1_Right");
    private Texture2D Weapon1Left => RequestTexture("Weapon1_Left");
    private Texture2D Weapon2Right => RequestTexture("Weapon2_Right");
    private Texture2D Weapon2Left => RequestTexture("Weapon2_Left");

    private Texture2D WeaponTexture(int direction) {
        return (HeroSlot == 0, direction == 1) switch {
            (true, true) => Weapon1Right,
            (true, false) => Weapon1Left,
            (false, true) => Weapon2Right,
            (false, false) => Weapon2Left
        };
    }

    public DrawData BuildDrawData(int direction) {
        var drawData = Player.BuildPlayerCenterDrawData(WeaponTexture(direction));
        drawData.destinationRectangle.FrontPlus(direction, Player.PlayerDrawHandOffset());
        drawData.origin = drawData.texture.TextureCenter().PlusX(-direction * (HeroSlot == 0 ? 6 : 12));
        return drawData;
    }
}