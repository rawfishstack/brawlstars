using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace brawlstars.Brawlstars;

/// <summary>
/// 每个英雄都可以继承此类用射弹实现行为。
/// </summary>
public abstract class BrawlerProjectile<T> : ModProjectile, IBrawlerBehaviorHolder<T> where T : BrawlerBehavior {
    public T Behavior { get; private set; }

    public void SetBehavior(BrawlerBehavior behavior) {
        Behavior = (T)behavior;
    }

    public Player Player => Behavior.Player;

    /// <summary>
    /// 英雄名称。
    /// </summary>
    public abstract string BrawlerName { get; }

    public override string LocalizationCategory => "Brawlers." + BrawlerName;

    public override string Name => BrawlerName + "_Projectile" + GetType().Name;

    /// <summary>
    /// 射弹的纹理一般仅用作调试，单独处理渲染。
    /// </summary>
    public override string Texture => (GetType().Namespace + "/Texture/" + GetType().Name).Replace('.', '/');


    public override void SetDefaults() {
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = 999999;
        Projectile.timeLeft = 24;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
    }

    public abstract void OnDepended();
}