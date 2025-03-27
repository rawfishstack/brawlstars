using System;
using System.Collections.Generic;
using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class AttackAction : BrawlerAction<Behavior> {
    public Vector2[] PositionTracks;
    public Vector2 Velocity;
    public int Direction;

    public void Begin() {
        PositionTracks = new Vector2[24];
        Velocity = Behavior.Player.PlayerMouseOffset();
        Velocity.Normalize();
        Velocity *= 16;

        // todo 速度过快导致轨迹不连续，可以插值以消除不连续
        if (Behavior.GadgetView.HasCooldown) {
            Velocity *= 4f / 3f;
        }

        Direction = Velocity.X > 0 ? 1 : -1;
    }


    public void SetVelocityAtEnd() {
        Velocity = Behavior.Player.velocity;
        Velocity.X.Clamp(-0.1f, 0.1f);
        Velocity.Y.Clamp(-8f, 8f);
    }

    public void ApplyG() {
        Velocity = Velocity.PlusY(Player.gravDir * Player.gravity * 2);
    }

    public void SetPlayerProperty() {
        Behavior.Player.velocity = Velocity;
    }


    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            var proj = Projectile.NewProjectile(
                new EntitySource_ItemUse(Player, Player.HeldItem),
                Player.position,
                Vector2.Zero,
                ModContent.ProjectileType<Attack>(),
                2000,
                0
            );
            Behavior.ProjectileDepending<Attack, Behavior>(proj);

            SoundEngine.PlaySound(Behavior.RequestSound("Attack"));

            Begin();

            if (Behavior.GadgetView.HasCooldown) {
                Player.itemAnimation = 18;
            }
        }

        if (Player.itemAnimation == 1) {
            SetVelocityAtEnd();
        }

        ApplyG();
        SetPlayerProperty();

        Player.direction = Direction;
        var drawData = Behavior.BuildDrawData(Direction);
        drawData.rotation += Direction * Math.Min(24 - Player.itemAnimation, 12) / 4f;
        drawInfo.DrawDataCache.Add(drawData);

        var baseIndex = 0;
        PositionTracks[24 - Player.itemAnimation] = Player.Center; // 0..23
        if (Player.itemAnimation < 13) {
            // 2 * [-1 13] * [i; 1]
            baseIndex = 2 * (13 - Player.itemAnimation); // 2..24 step 2
        }

        for (var i = baseIndex; i < 24; i++) {
            if (i == 0 || Behavior.GadgetView.HasCooldown && i <= 6) continue;
            var texture = Behavior.RequestTexture("Track_" + Math.Min(3, (i - 1) / 8 + 1));
            drawData = Player.BuildPlayerCenterDrawData(
                texture,
                Player.PlayerDrawCenterPosition(PositionTracks[i])
            );
            drawData.color = Color.White;
            drawData.origin = texture.TextureCenter();

            drawData.rotation = (PositionTracks[i] - PositionTracks[i - 1])
                .ToRotation();
            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}

public class Attack : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MORTIS";
    private readonly List<int> _attacked;

    public Attack() {
        _attacked = [];
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 40;
        Projectile.height = 60;
        Projectile.damage = 2000;
        Projectile.light = 1f;
        Projectile.alpha = 255;
    }

    public override void OnDepended() {
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        _attacked.Add(target.GetHashCode());
        if (!target.immortal) {
            Behavior.AddChargeValue(21.25f);
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
        Projectile.Center = Player.Center;
    }
}