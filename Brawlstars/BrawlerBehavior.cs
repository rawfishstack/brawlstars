using System;
using System.Collections.Generic;
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
        InitCategories();
        InitActionSlots();
        InitHyperCharge();
        InitActions();
        InitAllNotImplementationMarked();
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


    public Dictionary<BrawlerActionAggregate, BrawlerActionCategory> BrawlerActionCategories = new();

    protected virtual void InitCategories() {
        BrawlerActionCategories[BrawlerActionAggregate.Hero] = BrawlerActionCategory.One;
        BrawlerActionCategories[BrawlerActionAggregate.Gadget] = BrawlerActionCategory.Two;
        BrawlerActionCategories[BrawlerActionAggregate.StarPower] = BrawlerActionCategory.Two;
    }

    /// <summary>
    /// 极限充能效率，它是百分数。
    /// </summary>
    public Dictionary<BrawlerActionSlot, float> HyperChargeMultiplier = new();

    protected virtual void InitHyperCharge() {
        for (var i = 0; i < BrawlerActionCategories[BrawlerActionAggregate.Hero].Value; i++) {
            HyperChargeMultiplier[BrawlerActionSlot.Get(i)] = 0f;
        }
    }

    protected virtual void InitAllNotImplementationMarked() {
        NotImplementedAll = false;
        NotImplementedTraits = BrawlerActionCategories[BrawlerActionAggregate.Hero].Value.Array(_ => false);
        NotImplementedAttack = BrawlerActionCategories[BrawlerActionAggregate.Hero].Value.Array(_ => false);
        NotImplementedSuper = BrawlerActionCategories[BrawlerActionAggregate.Hero].Value.Array(_ => false);
        NotImplementedHyperCharge = BrawlerActionCategories[BrawlerActionAggregate.Hero].Value.Array(_ => false);
        NotImplementedGadget = BrawlerActionCategories[BrawlerActionAggregate.Gadget].Value.Array(_ => false);
        NotImplementedStarPower = BrawlerActionCategories[BrawlerActionAggregate.StarPower].Value.Array(_ => false);
    }

    // 未实现标记。
    public bool NotImplementedAll;
    public bool[] NotImplementedTraits;
    public bool[] NotImplementedAttack;
    public bool[] NotImplementedSuper;
    public bool[] NotImplementedHyperCharge;
    public bool[] NotImplementedGadget;
    public bool[] NotImplementedStarPower;

    public Dictionary<BrawlerActionAggregate, BrawlerActionSlot> BrawlerActionSlots = new();

    protected virtual void InitActionSlots() {
        HeroSlot = BrawlerActionSlot.First;
        GadgetSlot = BrawlerActionSlot.Second;
        StarPowerSlot = BrawlerActionSlot.Third;
    }

    public BrawlerActionSlot HeroSlot {
        get => BrawlerActionSlots[BrawlerActionAggregate.Hero];
        set => BrawlerActionSlots[BrawlerActionAggregate.Hero] = value;
    }

    public BrawlerActionSlot GadgetSlot {
        get => BrawlerActionSlots[BrawlerActionAggregate.Gadget];
        set => BrawlerActionSlots[BrawlerActionAggregate.Gadget] = value;
    }

    public BrawlerActionSlot StarPowerSlot {
        get => BrawlerActionSlots[BrawlerActionAggregate.StarPower];
        set => BrawlerActionSlots[BrawlerActionAggregate.StarPower] = value;
    }


    public Dictionary<BrawlerActionKey, IBrawlerAction<BrawlerBehavior>> BrawlerActions = new();

    protected virtual void InitActions() {
        GetType().GetFields()
            .ForEach(field => {
                    if (field.FieldType.IsSubclassOf(typeof(BrawlerAction<>).MakeGenericType(GetType()))) {
                        var s = field.Name;
                        foreach (var type in BrawlerActionType.All) {
                            if (s.StartsWith(type.Name)) {
                                var number = s[^1] - 49;
                                if (number is >= 0 and <= 9) {
                                } else {
                                    number = 0;
                                }

                                var instance = NewInstance(field.FieldType);
                                field.SetValue(this, instance);
                                BrawlerActions[new BrawlerActionKey(type, BrawlerActionSlot.Get(number))] = instance;

                                break;
                            }
                        }
                    }
                }
            );
    }

    private IBrawlerAction<BrawlerBehavior> NewInstance(Type type) {
        var action = (IBrawlerAction<BrawlerBehavior>)type.GetConstructor([]).Invoke([]);
        action.SetBehavior(this);
        return action;
    }

    public bool SuperUsed { get; private set; }
    public bool GadgetUsed { get; private set; }
    public GadgetView GadgetView { get; }
    public SuperView SuperView { get; }
    public HyperChargeView HyperChargeView { get; }
    public virtual ChargingView? ExtraView { get; }

    public virtual void AddChargeValue(float value) {
        SuperView.Charge += value;
        HyperChargeView.Charge += value * HyperChargeMultiplier[HeroSlot] / 100;
    }

    protected virtual void OnDraw(ref PlayerDrawSet drawInfo) {
        GadgetView.Update();
        SuperView.Update();
        HyperChargeView.Update();
        ExtraView?.Update();
    }

    public virtual void PostBrawlerAction(BrawlerActionType type, ref PlayerDrawSet drawInfo) {
        var key = new BrawlerActionKey(type, BrawlerActionSlots[type.Aggregate]);
        if (type == BrawlerActionType.Super) {
            if (!SuperUsed && SuperView.Next()) {
                SuperUsed = true;
            }
        }

        if (type == BrawlerActionType.Gadget) {
            if (!GadgetUsed && GadgetView.Next()) {
                GadgetUsed = true;
                SoundEngine.PlaySound(CommonSoundAssets.Instance.RequestSound(
                    Main.LocalPlayer == Player ? "PlayerUseGadget" : "EnemyUseGadget"
                ));
            }
        }

        BrawlerActions[key].PostAction(ref drawInfo);
        if (type == BrawlerActionType.Super) {
            if (Player.itemAnimation == 1) {
                SuperUsed = false;
            }
        }

        if (type == BrawlerActionType.Gadget) {
            if (Player.itemAnimation == 1) {
                GadgetUsed = false;
            }
        }
    }

    public virtual void PostGadgetSwitch() {
        BrawlerActions[
                new BrawlerActionKey(BrawlerActionType.Gadget, GadgetSlot)]
            .PostDisable();
        GadgetSlot = GadgetSlot
            .Next(BrawlerActionCategories[BrawlerActionAggregate.Gadget]);
        BrawlerActions[
                new BrawlerActionKey(BrawlerActionType.Gadget, GadgetSlot)]
            .PostEnable();
        GadgetView.Reset();
        GadgetUsed = false;
    }

    public virtual void PostHyperChargeAction(ref PlayerDrawSet drawInfo) {
        // todo: 极限充能模型未实现
    }

    public virtual void PostStarPowerSwitch() {
        BrawlerActions[
                new BrawlerActionKey(BrawlerActionType.StarPower, StarPowerSlot)]
            .PostDisable();
        StarPowerSlot = StarPowerSlot
            .Next(BrawlerActionCategories[BrawlerActionAggregate.StarPower]);
        BrawlerActions[
                new BrawlerActionKey(BrawlerActionType.StarPower, StarPowerSlot)]
            .PostEnable();
    }


    public virtual void PostDraw(ref PlayerDrawSet drawInfo) {
        OnDraw(ref drawInfo);
    }

    public virtual void Enable() {
        GadgetView.Reset();
        BrawlerActionType.All.ForEach(type =>
            BrawlerActions.GetValueOrDefault(new BrawlerActionKey(type, BrawlerActionSlots[type.Aggregate]))
                ?.PostEnable());
    }

    public virtual void Disable() {
        BrawlerActionType.All.ForEach(type =>
            BrawlerActions.GetValueOrDefault(new BrawlerActionKey(type, BrawlerActionSlots[type.Aggregate]))
                ?.PostDisable());
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