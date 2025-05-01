using brawlstars.Brawlstars.Brawlers.Meg;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public class BrawlerPlayer : ModPlayer {
    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust,
        ref PlayerDeathReason damageSource) {
        if (BrawlerBehavior.FromPlayerHeld(Player) is Behavior behavior) {
            if (behavior.BrawlerActionSlots[BrawlerActionAggregate.Hero] == BrawlerActionSlot.Second) {
                playSound = false;
                genDust = false;
                behavior.BrawlerActionSlots[BrawlerActionAggregate.Hero] = BrawlerActionSlot.First;
                Player.Heal(500);
                return false;
            }
        }

        return true;
    }
}