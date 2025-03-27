#nullable enable
using brawlstars.Brawlstars.Common.Systems;
using brawlstars.Brawlstars.Common.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public class BrawlerDrawLayer : PlayerDrawLayer {
    private Action _action = Action.Static;

    private ModItem? _currentItem;
    private ModItem? _oldItem;

    public override Position GetDefaultPosition() {
        return new Between();
    }


    public Vector2 Target;

    // 主循环。
    protected override void Draw(ref PlayerDrawSet drawInfo) {
        if (drawInfo.shadow != 0f) {
            return;
        }

        _currentItem = drawInfo.drawPlayer.HeldItem.ModItem;
        if (_currentItem is BrawlerItem brawlerItem) {
            if (_oldItem != _currentItem) {
                (_oldItem as BrawlerItem)?.Behavior.Disable();
                brawlerItem.Behavior.Enable();
                ModContent.GetInstance<Ui>().Hide();
                ModContent.GetInstance<Ui>().Show(brawlerItem);
            }

            if (!drawInfo.drawPlayer.dead) {
                brawlerItem.Behavior.Player = drawInfo.drawPlayer;
                brawlerItem.Behavior.PostDraw(ref drawInfo);
                AllocateAnimation(brawlerItem, ref drawInfo);
            }
        } else {
            ModContent.GetInstance<Ui>().Hide();
            (_oldItem as BrawlerItem)?.Behavior.Disable();
        }

        _oldItem = _currentItem;
    }

    private void AllocateAnimation(BrawlerItem brawlerItem, ref PlayerDrawSet drawInfo) {
        var player = drawInfo.drawPlayer;
        if (player.itemAnimation == 0) _action = Action.Static;

        if (_action == Action.Static) {
            _action = Controlled(player);
            if (_action != Action.Static) ApplyUseAnimation(player, brawlerItem);
        }

        switch (_action) {
            case Action.Static: brawlerItem.Behavior.PostStaticAction(ref drawInfo); break;
            case Action.Attack: brawlerItem.Behavior.PostAttackAction(ref drawInfo); break;
            case Action.Super: brawlerItem.Behavior.PostSuperAction(ref drawInfo); break;
            case Action.Gadget: brawlerItem.Behavior.PostGadgetAction(ref drawInfo); break;
            case Action.HyperCharge: brawlerItem.Behavior.PostHyperChargeAction(ref drawInfo); break;
            default: break;
        }

        if (KeyBinds.GadgetSelectKeybind.JustPressed) brawlerItem.Behavior.PostGadgetSwitch();

        if (KeyBinds.StarPowerSelectKeybind.JustPressed) brawlerItem.Behavior.PostStarPowerSwitch();
    }

    private static Action Controlled(Player player) {
        if (player.itemAnimation > 0) {
            if (player.altFunctionUse == 0) return Action.Attack;

            if (player.altFunctionUse == 2) return Action.Super;
        }

        if (KeyBinds.GadgetUseKeybind.JustPressed) return Action.Gadget;

        if (KeyBinds.HyperChargeUseKeybind.JustPressed) return Action.HyperCharge;

        return Action.Static;
    }


    private static void ApplyUseAnimation(Player player, ModItem modItem) {
        player.itemAnimation = modItem.Item.useAnimation;
    }

    private enum Action {
        Static,
        Attack,
        Super,
        Gadget,
        HyperCharge
    }
}