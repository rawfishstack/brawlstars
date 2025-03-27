using System.Collections.Generic;
using System.Linq;
using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Brawlers.Meg;

public class SuperAction2 : BrawlerAction<Behavior> {
    public Vector2 Target;
    public Vector2 TargetScope;
    public int Direction;

    public float[] Rotations;
    public float[] ShadowRotations;

    public void Begin() {
        Target = Behavior.Player.PlayerMouseOffset();
        TargetScope = Target;
        TargetScope.Normalize();
        TargetScope *= 240;
        Direction = Target.X > 0 ? 1 : -1;

        Rotations = new float[24];

        // 单侧最大角度
        const float rad = 2.1f;

        for (var i = 0; i < 24; i++) {
            if (i < 12) {
                Rotations[i] = -0.5f * rad + 1.5f * rad * i / 11f;
            } else if (i < 16) {
                Rotations[i] = rad;
            } else {
                Rotations[i] = -rad;
            }
        }

        ShadowRotations = new float[12];
        for (var i = 0; i < 12; i++) {
            ShadowRotations[i] = rad - 2 * rad * (i + 1) / 13;
        }
    }

    protected override void OnAction(ref PlayerDrawSet drawInfo) {
        if (Player.itemAnimation == 24) {
            SoundEngine.PlaySound(Behavior.RequestSound("Super2"));

            Begin();
        }

        if (Player.itemAnimation == 6) {
            var proj = Projectile.NewProjectile(
                new EntitySource_ItemUse(Player, Player.HeldItem),
                Player.position,
                Vector2.Zero,
                ModContent.ProjectileType<Super2>(),
                3400,
                0
            );
            Behavior.ProjectileDepending<Super2, Behavior>(proj);
        }

        Player.direction = Direction;
        DrawData drawData;

        if (Player.itemAnimation < 9) {
            for (var i = 0; i < 5; i++) {
                var index = 8 - Player.itemAnimation + i; // 0..11

                drawData = Behavior.BuildDrawData(Direction);
                drawData.rotation += Target.ToRotation() + ShadowRotations[index];
                if (Direction == -1) {
                    drawData.rotation += -3.14f;
                }

                float blend = 0.1f + i * 0.05f;
                drawData.color = new Color(blend, blend, blend, blend);
                drawInfo.DrawDataCache.Add(drawData);

                var scale = 2f * index / 11;
                drawData = Player.BuildPlayerCenterDrawData(Behavior.RequestTexture("Super2_Track"));
                drawData.origin = drawData.texture.TextureCenter();
                var t = TargetScope.RotatedBy(ShadowRotations[index]);
                drawData.destinationRectangle.X += (int)t.X;
                drawData.destinationRectangle.Y += (int)t.Y;
                drawData.destinationRectangle.Width = (int)(drawData.destinationRectangle.Width * scale);
                drawData.destinationRectangle.Height = (int)(drawData.destinationRectangle.Height * 1.5f);
                drawData.rotation = t.ToRotation();
                drawInfo.DrawDataCache.Add(drawData);

                scale *= 2;
                Dust.NewDust(
                    Player.Center - new Vector2(40, 40) + t,
                    80, 80,
                    ModContent.DustType<Dust1>(),
                    Scale: scale
                );
            }
        }

        drawData = Behavior.BuildDrawData(Direction);
        drawData.rotation += Target.ToRotation() + Rotations[24 - Player.itemAnimation];
        if (Direction == -1) {
            drawData.rotation += -3.14f;
        }

        drawInfo.DrawDataCache.Add(drawData);
    }
}

public class Super2 : BrawlerProjectile<Behavior> {
    public override string BrawlerName => "MEG";
    private readonly List<int> _attacked;

    private Vector2 _targetScope;
    private float _targetDistance;

    public Super2() {
        _attacked = [];
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Projectile.timeLeft = 6;
        Projectile.width = 520;
        Projectile.height = 520;
        Projectile.damage = 3400;
        Projectile.light = 5f;
        Projectile.alpha = 255;
    }

    public override void OnDepended() {
        _targetScope = Behavior.SuperAction2.TargetScope;
        _targetDistance = _targetScope.Length();
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
        _attacked.Add(target.GetHashCode());
        if (!target.immortal) {
            Behavior.AddChargeValue(21.25f);
        }
    }

    public override bool? CanHitNPC(NPC target) {
        return Projectile.timeLeft == 1 &&
               !_attacked.Contains(target.GetHashCode()) &&
               !target.friendly &&
               target.Center.Distance(Player.Center) < _targetDistance &&
               NoBlock(target);
    }

    public bool NoBlock(NPC target) {
        return true;

        var tc = target.Center;
        var pc = Player.Center;
        var c = tc - pc;
        if (c.Length() < 40) {
            return true;
        }

        c.Normalize();
        c *= 16;
        tc -= c;
        pc += c;
        var x1 = pc.X;
        var x2 = tc.X;
        var y1 = pc.Y;
        var y2 = tc.Y;

        var points = 10.List(i => {
            var t = i / 10f;
            return new Point((int)((x1 + t * (x2 - x1)) / 16), (int)((y1 + t * (y2 - y1)) / 16));
        });

        return points.All(point => Main.tile[point].TileType == 0);
        // 找不到判断瓷砖可通过的方法 无法实现阻挡攻击
    }

    public override void AI() {
        Projectile.Center = Player.Center;
        Projectile.rotation = _targetScope.ToRotation();
        Projectile.Center += _targetScope;
    }
}