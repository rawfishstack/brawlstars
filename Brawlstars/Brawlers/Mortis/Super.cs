using System.Collections.Generic;
using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class SuperAction : BrawlerAction<Behavior> {
    public Vector2 Velocity;
    public int Direction;

    public void Begin() {
        Velocity = Behavior.Player.PlayerMouseOffset();
        Velocity.Normalize();
        Velocity *= 16;
        Direction = Velocity.X > 0 ? 1 : -1;
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            Begin();
        }

        if (!Behavior.SuperUsed) {
            if (Player.itemAnimation == 24) {
                Player.itemAnimation = 6;
            }
        }

        if (Player.itemAnimation == 24) {
            var proj = Projectile.NewProjectile(
                new EntitySource_ItemUse(Player, Player.HeldItem),
                Player.Center,
                Velocity,
                ModContent.ProjectileType<Super>(),
                1800,
                0
            );
            Behavior.ProjectileDepending<Super, Behavior>(proj);

            SoundEngine.PlaySound(Behavior.RequestSound("Super"));
        }

        Player.direction = Direction;

        var drawData = Behavior.BuildDrawData(Direction);
        drawData.rotation = -Direction * 0.2f;
        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class Super : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MORTIS";
    private readonly List<int> _attacked;

    public Super() {
        _attacked = [];
    }

    public override void SetStaticDefaults() {
        Main.projFrames[Projectile.type] = 3;
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 96;
        Projectile.height = 96;
        Projectile.timeLeft = 60;
        Projectile.damage = 1800;
        Projectile.light = 1;
        Projectile.frame = 0;
    }

    public override void OnDepended() {
    }


    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        _attacked.Add(target.GetHashCode());
        if (!target.immortal) {
            Player.Heal(((int)(target.lifeMax * 1.25f)).ToClamp(75, 225));
            Behavior.AddChargeValue(30.15f);
        }

        if (target.life <= 0) {
            Behavior.StarPowerAction1.Heal();
        }
    }

    public override bool? CanHitNPC(NPC target) {
        return !_attacked.Contains(target.GetHashCode()) &&
               !target.friendly;
    }

    public override void AI() {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.frame++;
        if (Projectile.frame >= Main.projFrames[Projectile.type]) Projectile.frame = 0;
    }
}