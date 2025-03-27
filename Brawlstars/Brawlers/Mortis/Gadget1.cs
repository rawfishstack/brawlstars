using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Mortis;

public class GadgetAction1 : BrawlerAction<Behavior> {
    public Vector2 Target;
    public int Direction;

    public void Begin() {
        Target = Behavior.Player.PlayerMouseOffset();
        Direction = Target.X > 0 ? 1 : -1;
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (!Behavior.GadgetUsed) {
            if (Player.itemAnimation == 24) Player.itemAnimation = 6;
            Behavior.StaticAction.PostAction(ref drawInfo);
            return;
        }

        if (Player.itemAnimation == 24) {
            Begin();
            var proj = Projectile.NewProjectile(
                new EntitySource_ItemUse(Player, Player.HeldItem),
                Player.position + new Vector2(10, 14),
                Vector2.Zero,
                ModContent.ProjectileType<Gadget1>(),
                1800,
                0
            );
            Behavior.ProjectileDepending<Gadget1, Behavior>(proj);

            SoundEngine.PlaySound(Behavior.RequestSound("Gadget1"));

            Player.itemAnimation = 12;
        }


        var direction = Direction;
        Player.direction = direction;
        var drawData = Behavior.BuildDrawData(direction);
        drawData.rotation = direction * Player.itemAnimation * 0.4f;
        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class Gadget1 : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MORTIS";
    private readonly List<int> _attacked;

    public Gadget1() {
        _attacked = [];
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.width = 96;
        Projectile.height = 96;
        Projectile.timeLeft = 12;
        Projectile.damage = 1800;
        Projectile.light = 1f;
        Projectile.alpha = 255;
    }

    public override void OnDepended() {
    }


    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        _attacked.Add(target.GetHashCode());
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