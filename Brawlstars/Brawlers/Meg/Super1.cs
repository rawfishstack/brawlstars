using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class SuperAction1 : BrawlerAction<Behavior> {
    public int Direction;

    public void Begin() {
        Direction = Behavior.Player.PlayerMouseOffset().X > 0 ? 1 : -1;
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            Begin();
        }

        if (!Behavior.SuperUsed) {
            if (Player.itemAnimation == 24) {
                Player.itemAnimation = 3;
            }
        } else {
            Player.direction = Direction;
            if (Player.itemAnimation == 24) {
                SoundEngine.PlaySound(Behavior.RequestSound("Super1"));

                Player.Heal(500);
            }

            if (Player.itemAnimation == 1) {
                Behavior.BrawlerActionSlots[BrawlerActionAggregate.Hero] = BrawlerActionSlot.Second;
            }

            if (Player.itemAnimation % 3 == 0) {
                Direction *= -1;
            }

            Dust.NewDust(
                Player.Center - new Vector2(20, -20),
                40, 20,
                ModContent.DustType<Dust1>(),
                0, -2,
                Scale: 1.7f
            );
            var drawData = Behavior.BuildDrawData(Direction);
            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}