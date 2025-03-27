using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class AttackAction1 : BrawlerAction<Behavior> {
    public Vector2 Velocity;
    public Vector2 Unit;
    public int Direction;

    public void Begin() {
        Unit = Behavior.Player.PlayerMouseOffset().PlusY(6);
        Unit.Normalize();
        Velocity = Unit * 24;
        Direction = Velocity.X > 0 ? 1 : -1;
    }

    public void Shot() {
        var proj = Projectile.NewProjectile(
            new EntitySource_ItemUse(Player, Player.HeldItem),
            Player.Center.PlusY(-6) - Unit * 12,
            Velocity,
            ModContent.ProjectileType<Attack1>(),
            600,
            0
        );
        Behavior.ProjectileDepending<Attack1, Behavior>(proj);

        SoundEngine.PlaySound(Behavior.RequestSound("Attack1"));

        Dust.NewDust(
            Player.Center - new Vector2(20, 20),
            40, 40,
            ModContent.DustType<Dust1>(),
            Velocity.X / 4, Velocity.Y / 4,
            Scale: 1.3f
        );
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            Begin();
            Shot();
        }

        if (Player.itemAnimation == 20) {
            Shot();
            Player.itemAnimation = 12;
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

public class Attack1 : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MEG";

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 32;
        Projectile.height = 32;
        Projectile.damage = 600;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 36;
        Projectile.tileCollide = true;
        Projectile.light = 1f;
        Projectile.alpha = 0;
    }

    public override void OnDepended() {
        Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        if (!target.immortal) {
            Behavior.AddChargeValue(13.5f);
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