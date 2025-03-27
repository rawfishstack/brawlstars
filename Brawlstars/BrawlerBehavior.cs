using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using brawlstars.Brawlstars.BrawlerContent.CommonSound;
using brawlstars.Brawlstars.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars;

/// <summary>
/// 每个英雄的行为。
/// 应每个命名空间仅有一个此类的子类且命名为"Behavior"。
/// </summary>
public abstract class BrawlerBehavior : AssetsSpace {
    /// <summary>
    /// 由外部赋值。
    /// </summary>
    public Player Player;
#nullable enable

    public readonly BrawlerItem Item;

    protected BrawlerBehavior(BrawlerItem item) {
        Item = item;
        GadgetView = new GadgetView(this);
        SuperView = new SuperView(this);
        HyperChargeView = new HyperChargeView(this);

        Construct();
    }

    private void Construct() {
        BeginBuildInfo();
        CalCategories();
        InitHyperCharge();
        InitAllNotImplementationMarked();
        ConstructAction();
    }

    public BrawlerInfo BrawlerInfo;

    protected void BeginBuildInfo() {
        var builder = new BrawlerInfo.BrawlerInfoBuilder(() => Item.Mod, Item.BrawlerName, Item.LocalizationCategory);
        BuildInfo(builder);
        BrawlerInfo = builder.Build();

        // check
        if (BrawlerInfo.HeroInfos.Length == 0) {
            throw new Exception($"{Item.BrawlerName} Hero info is empty!");
        }
    }

    protected virtual void BuildInfo(BrawlerInfo.BrawlerInfoBuilder builder) {
    }


    protected virtual void CalCategories() {
        HeroCategory = BrawlerInfo.HeroInfos.Length;
        GadgetCategory = BrawlerInfo.GadgetInfos.Length;
        StarPowerCategory = BrawlerInfo.StarPowerInfos.Length;
    }

    /// <summary>
    /// 极限充能效率，它是百分数。
    /// </summary>
    public float[] HyperChargeMultiplier;

    protected virtual void InitHyperCharge() {
        HyperChargeMultiplier = HeroCategory.Array(_ => 0f);
    }

    protected virtual void InitAllNotImplementationMarked() {
        NotImplementedAll = false;
        NotImplementedTraits = HeroCategory.Array(_ => false);
        NotImplementedAttack = HeroCategory.Array(_ => false);
        NotImplementedSuper = HeroCategory.Array(_ => false);
        NotImplementedHyperCharge = HeroCategory.Array(_ => false);
        NotImplementedGadget = GadgetCategory.Array(_ => false);
        NotImplementedStarPower = StarPowerCategory.Array(_ => false);
    }

    /// <summary>
    /// 英雄形态种类，默认是1。
    /// Traits，Attack，Super，HyperCharge的未实现标记的数组长度必须达到这个值。
    /// </summary>
    public int HeroCategory { get; protected set; }

    /// <summary>
    /// 妙具种类，默认是2。
    /// Gadget的未实现标记的数组长度必须达到这个值。
    /// </summary>
    public int GadgetCategory { get; protected set; }

    /// <summary>
    /// 星辉种类，默认是2。
    /// StarPower的未实现标记的数组长度必须达到这个值。
    /// </summary>
    public int StarPowerCategory { get; protected set; }

    // 未实现标记。
    public bool NotImplementedAll;
    public bool[] NotImplementedTraits;
    public bool[] NotImplementedAttack;
    public bool[] NotImplementedSuper;
    public bool[] NotImplementedHyperCharge;
    public bool[] NotImplementedGadget;
    public bool[] NotImplementedStarPower;

    public bool SuperUsed { get; private set; }
    public bool GadgetUsed { get; private set; }

    // 不能超过对应物品的对应装备的种类。
    public int HeroSlot = 0;
    public int GadgetSlot = 0;
    public int StarPowerSlot = 0;


    public GadgetView GadgetView { get; }
    public SuperView SuperView { get; }
    public HyperChargeView HyperChargeView { get; }
    public virtual ChargingView? ExtraView { get; }

    public virtual void AddChargeValue(float value) {
        SuperView.Charge += value;
        HyperChargeView.Charge += value * HyperChargeMultiplier[HeroSlot] / 100;
    }

    private IBrawlerAction<BrawlerBehavior> NewInstance(Type type) {
        var action = (IBrawlerAction<BrawlerBehavior>)type.GetConstructor([]).Invoke([]);
        action.SetBehavior(this);
        return action;
    }

    protected virtual void ConstructAction() {
        GetType().GetFields()
            .ForEach(field => {
                    Console.WriteLine(field.FieldType.IsSubclassOf(typeof(BrawlerAction<>).MakeGenericType(GetType())));
                }
            );
    }

    protected virtual void OnDraw(ref PlayerDrawSet drawInfo) {
        GadgetView.Update();
        SuperView.Update();
        HyperChargeView.Update();
        ExtraView?.Update();
    }

    public virtual void PostStaticAction(ref PlayerDrawSet drawInfo) {
        //StaticActionInstance[HeroSlot].PostAction(ref drawInfo);
    }

    public virtual void PostAttackAction(ref PlayerDrawSet drawInfo) {
        //AttackActionInstance[HeroSlot].PostAction(ref drawInfo);
    }

    public virtual void PostSuperAction(ref PlayerDrawSet drawInfo) {
        if (!SuperUsed && SuperView.Next()) {
            SuperUsed = true;
        }

        //SuperActionInstance[HeroSlot].PostAction(ref drawInfo);

        if (Player.itemAnimation == 1) {
            SuperUsed = false;
        }
    }

    public virtual void PostGadgetAction(ref PlayerDrawSet drawInfo) {
        if (!GadgetUsed && GadgetView.Next()) {
            GadgetUsed = true;
            SoundEngine.PlaySound(CommonSoundAssets.Instance.RequestSound(
                Main.LocalPlayer == Player ? "PlayerUseGadget" : "EnemyUseGadget"
            ));
        }

        //GadgetActionInstance[GadgetSlot].PostAction(ref drawInfo);

        if (Player.itemAnimation == 1) {
            GadgetUsed = false;
        }
    }

    public virtual void PostGadgetSwitch() {
        //GadgetActionInstance[GadgetSlot].PostDisable();
        GadgetSlot++;
        GadgetSlot %= GadgetCategory;
        //GadgetActionInstance[GadgetSlot].PostEnable();
        GadgetView.Reset();
        GadgetUsed = false;
    }

    public virtual void PostHyperChargeAction(ref PlayerDrawSet drawInfo) {
        // todo: 极限充能模型未实现
    }

    public virtual void PostStarPowerSwitch() {
        //StarPowerActionInstance[StarPowerSlot].PostDisable();
        StarPowerSlot++;
        StarPowerSlot %= StarPowerCategory;
        //StarPowerActionInstance[StarPowerSlot].PostEnable();
    }


    public virtual void PostDraw(ref PlayerDrawSet drawInfo) {
        OnDraw(ref drawInfo);
    }

    public virtual void Enable() {
        GadgetView.Reset();
        // AttackActionInstance[HeroSlot].PostEnable();
        // SuperActionInstance[HeroSlot].PostEnable();
        // HyperChargeActionInstance[HeroSlot].PostEnable();
        // GadgetActionInstance[GadgetSlot].PostEnable();
        // StarPowerActionInstance[StarPowerSlot].PostEnable();
    }

    public virtual void Disable() {
        // AttackActionInstance[HeroSlot].PostDisable();
        // SuperActionInstance[HeroSlot].PostDisable();
        // HyperChargeActionInstance[HeroSlot].PostDisable();
        // GadgetActionInstance[GadgetSlot].PostDisable();
        // StarPowerActionInstance[StarPowerSlot].PostDisable();
    }

    public P? ProjectileDepending<P, T>(int projectileIndex) where T : BrawlerBehavior where P : BrawlerProjectile<T> {
        if (Main.projectile[projectileIndex].ModProjectile is not P projectile) {
            return null;
        }

        projectile.SetBehavior(this);
        projectile.OnDepended();
        return projectile;
    }

    public static BrawlerBehavior? FromPlayerHeld(Player player) {
        return player.HeldItem.ModItem is BrawlerItem brawlerItem ? brawlerItem.Behavior : null;
    }
}