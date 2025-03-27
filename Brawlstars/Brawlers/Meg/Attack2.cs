using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class AttackAction2 : BrawlerAction<Behavior> {
    public Vector2 Velocity;
    public Vector2 Unit;
    public int Direction;

    public void Begin() {
        Unit = Behavior.Player.PlayerMouseOffset().PlusY(2);
        Unit.Normalize();
        Velocity = Unit * 27;
        Direction = Velocity.X > 0 ? 1 : -1;
    }

    public void Shot(int position) {
        var offset = Unit.RotatedBy(position * double.Pi / 2) * 19;
        var proj = Projectile.NewProjectile(
            new EntitySource_ItemUse(Player, Player.HeldItem),
            Player.Center.PlusY(-2) + offset,
            Velocity.RotatedBy(0.06f),
            ModContent.ProjectileType<Attack2>(),
            300,
            0
        );
        Behavior.ProjectileDepending<Attack2, Behavior>(proj);

        proj = Projectile.NewProjectile(
            new EntitySource_ItemUse(Player, Player.HeldItem),
            Player.Center.PlusY(-2) + offset,
            Velocity.RotatedBy(-0.06f),
            ModContent.ProjectileType<Attack2>(),
            300,
            0
        );
        Behavior.ProjectileDepending<Attack2, Behavior>(proj);

        if (position == 1) {
            SoundEngine.PlaySound(Behavior.RequestSound("Attack2"));
        }

        Dust.NewDust(
            Player.Center - new Vector2(10, 10) + offset,
            20, 20,
            ModContent.DustType<Dust1>(),
            Velocity.X / 4, Velocity.Y / 4,
            Scale: 1.5f
        );
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            Begin();
        }

        if (Player.itemAnimation is 24 or 18 or 12 or 6) {
            Shot(1);
        } else if (Player.itemAnimation is 21 or 15 or 9 or 3) {
            Shot(-1);
        }

        Player.direction = Direction;
        var drawData = Behavior.BuildDrawData(Direction);
        drawData.rotation = Velocity.ToRotation();
        if (Direction == -1) {
            drawData.rotation += float.Pi;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class Attack2 : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MEG";

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.damage = 300;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 30;
        Projectile.tileCollide = true;
        Projectile.light = 1f;
        Projectile.alpha = 0;
    }

    public override void OnDepended() {
        Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (!target.immortal) {
            Behavior.AddChargeValue(6.75f);
        }

        Projectile.Kill();
    }

    public override bool? CanHitNPC(NPC target) {
        return !target.friendly;
    }

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough,
        ref Vector2 hitboxCenterFrac) {
        width = 16;
        height = 16;
        return true;
    }

    public override void AI() {
    }
}