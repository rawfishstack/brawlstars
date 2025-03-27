using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Bibi;

public class SuperAction : BrawlerAction<Behavior> {
    public Vector2 Velocity;
    public int Direction;

    public void Begin() {
        Velocity = Behavior.Player.PlayerMouseOffset();
        Velocity.Normalize();
        Velocity *= 13.6f;
        Direction = Velocity.X > 0 ? 1 : -1;
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (!Behavior.SuperUsed) {
            if (Player.itemAnimation == 24) {
                Player.itemAnimation = 6;
                Begin();
            }
        }

        if (Player.itemAnimation == 24) {
            Begin();

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
    public override string BrawlerName => "BIBI";
    private readonly Dictionary<int, int> _attacked;

    public Super() {
        _attacked = [];
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.timeLeft = 160;
        Projectile.damage = 1800;
        Projectile.light = 1;
        Projectile.tileCollide = true;
    }

    public override void OnDepended() {
    }


    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        _attacked[target.GetHashCode()] = Projectile.timeLeft;
        if (!target.immortal) {
            Behavior.AddChargeValue(25.2f);
        }

        SoundEngine.PlaySound(Behavior.RequestSound("Super_Hit"));
    }

    public override bool? CanHitNPC(NPC target) {
        return _attacked.GetValueOrDefault(target.GetHashCode(), int.MaxValue) - Projectile.timeLeft > 20 &&
               !target.friendly;
    }

    public override bool OnTileCollide(Vector2 oldVelocity) {
        if (Projectile.velocity.X != oldVelocity.X) {
            Projectile.position.X += Projectile.velocity.X;
            Projectile.velocity.X = 0f - oldVelocity.X;
        }

        if (Projectile.velocity.Y != oldVelocity.Y) {
            Projectile.position.Y += Projectile.velocity.Y;
            Projectile.velocity.Y = 0f - oldVelocity.Y;
        }

        SoundEngine.PlaySound(Behavior.RequestSound("Super_Wall"));

        return false;
    }

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough,
        ref Vector2 hitboxCenterFrac) {
        width = 16;
        height = 16;
        return true;
    }

    public override void AI() {
        Projectile.rotation += 0.1f;
        if (Projectile.timeLeft == 1) {
            SoundEngine.PlaySound(Behavior.RequestSound("Super_End"));
        }
    }
}