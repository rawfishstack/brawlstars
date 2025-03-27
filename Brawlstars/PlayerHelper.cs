using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;

namespace brawlstars.Brawlstars;

public static class PlayerHelper {
    /// <summary>
    /// 玩家悬空。
    /// </summary>
    /// <returns></returns>
    public static bool PlayerPoseIsHover(this Player player) {
        return player.bodyFrame.Y == 280;
    }

    /// <summary>
    /// 玩家移动。
    /// </summary>
    /// <returns>moving 1..14 or not moving 0</returns>
    public static bool PlayerPoseIsMove(this Player player) {
        return player.PlayerPoseMoveStatus() > 0;
    }

    /// <summary>
    /// 玩家使用物品。
    /// </summary>
    /// <returns></returns>
    public static bool PlayerPoseUseItem(this Player player) {
        return player.itemAnimation > 0;
    }

    /// <summary>
    /// 玩家移动状态。
    /// 也包括静止状态。
    /// </summary>
    /// <returns>moving 1..13 or not moving 0</returns>
    public static int PlayerPoseMoveStatus(this Player player) {
        if (player.legFrame.Y is >= 392 and <= 1064) {
            return (player.legFrame.Y - 392) / 56 + 1;
        }

        return 0;
    }

    private static readonly Vector2[] PlayerMovingDrawHandOffsets = [
        new(-8, 10), // static
        new(-4, 6),
        new(-4, 6),
        new(-4, 6),
        new(-4, 8),
        new(-2, 8),
        new(-2, 8),
        new(-2, 8),
        new(0, 6),
        new(2, 6),
        new(2, 6),
        new(0, 8),
        new(-2, 8),
        new(-2, 8),
    ];


    private static readonly Vector2 PlayerHoveringDrawHandOffset = new(-10, -12);
    private static readonly Vector2 PlayerUsingItemDrawHandOffset = new(6, 6);

    /// <summary>
    /// 玩家手相对玩家绘制中心的偏移。
    /// </summary>
    /// <returns></returns>
    public static Vector2 PlayerDrawHandOffset(this Player player) {
        if (PlayerPoseUseItem(player)) {
            return PlayerUsingItemDrawHandOffset;
        }

        if (PlayerPoseIsHover(player)) {
            return PlayerHoveringDrawHandOffset;
        }

        return PlayerMovingDrawHandOffsets[PlayerPoseMoveStatus(player)];
    }

    /// <summary>
    /// 玩家绘制区的中心的位置。
    /// </summary>
    /// <returns></returns>
    public static Vector2 PlayerDrawCenterPosition(this Player player) {
        return PlayerDrawCenterPosition(player, player.Center);
    }

    public static Vector2 PlayerDrawCenterPosition(this Player player, Vector2 playerCenterPosition) {
        return playerCenterPosition - Main.screenPosition;
    }

    /// <summary>
    /// 玩家位置的灯光。
    /// </summary>
    /// <returns></returns>
    public static Color PlayerLighting(this Player player) {
        var playerPosition = player.position;
        return Lighting.GetColor((int)playerPosition.X / 16, (int)playerPosition.Y / 16);
    }

    /// <summary>
    /// 在玩家绘制区中心创建纹理的绘制数据。
    /// </summary>
    /// <returns></returns>
    public static DrawData BuildPlayerCenterDrawData(this Player player, Texture2D texture) {
        return BuildPlayerCenterDrawData(player, texture, PlayerDrawCenterPosition(player));
    }

    public static DrawData BuildPlayerCenterDrawData(this Player player, Texture2D texture,
        Vector2 playerDrawCenter) {
        return new DrawData(
            texture,
            new Rectangle(
                (int)playerDrawCenter.X, (int)playerDrawCenter.Y,
                // 大小只有原版的0.5倍，所以补充到2倍。
                texture.Width * 2, texture.Height * 2
            ),
            PlayerLighting(player)
        );
    }

    /// <summary>
    /// 玩家鼠标相对玩家的向量。
    /// </summary>
    /// <returns></returns>
    public static Vector2 PlayerMouseOffset(this Player player) {
        return Main.MouseWorld.ToWorldCoordinates() / 16 - player.Center;
    }
}