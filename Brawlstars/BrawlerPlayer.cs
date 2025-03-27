using brawlstars.Brawlstars.Brawlers.Meg;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public class BrawlerPlayer : ModPlayer {
    public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust,
        ref PlayerDeathReason damageSource) {
        if (BrawlerBehavior.FromPlayerHeld(Player) is Behavior behavior) {
            if (behavior.HeroSlot == 1) {
                playSound = false;
                genDust = false;
                behavior.HeroSlot = 0;
                Player.Heal(500);
                return false;
            }
        }

        return true;
    }
}