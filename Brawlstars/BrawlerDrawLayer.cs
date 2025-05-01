#nullable enable
using brawlstars.Brawlstars.Common.Systems;
using brawlstars.Brawlstars.Common.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars;

public class BrawlerDrawLayer : PlayerDrawLayer {
    private BrawlerActionType _action = BrawlerActionType.Static;

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
        if (player.itemAnimation == 0) _action = BrawlerActionType.Static;

        if (_action == BrawlerActionType.Static) {
            _action = Controlled(player);
            if (_action != BrawlerActionType.Static) ApplyUseAnimation(player, brawlerItem);
        }

        brawlerItem.Behavior.PostBrawlerAction(_action, ref drawInfo);

        if (KeyBinds.GadgetSelectKeybind.JustPressed) brawlerItem.Behavior.PostGadgetSwitch();

        if (KeyBinds.StarPowerSelectKeybind.JustPressed) brawlerItem.Behavior.PostStarPowerSwitch();
    }

    private static BrawlerActionType Controlled(Player player) {
        if (player.itemAnimation > 0) {
            if (player.altFunctionUse == 0) return BrawlerActionType.Attack;

            if (player.altFunctionUse == 2) return BrawlerActionType.Super;
        }

        if (KeyBinds.GadgetUseKeybind.JustPressed) return BrawlerActionType.Gadget;

        if (KeyBinds.HyperChargeUseKeybind.JustPressed) return BrawlerActionType.HyperCharge;

        return BrawlerActionType.Static;
    }


    private static void ApplyUseAnimation(Player player, ModItem modItem) {
        player.itemAnimation = modItem.Item.useAnimation;
    }
}