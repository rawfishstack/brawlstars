using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace brawlstars;

public class BrawlStars : Mod {
    public static BrawlStars Instance { get; private set; }

    public BrawlStars() {
        Instance = this;
    }

    public void SendTargetVector() {
        var playerPosition = Main.CurrentPlayer.position;
        var mousePostion = Main.MouseWorld.ToWorldCoordinates();

        var d = mousePostion / 16 - (playerPosition + new Vector2(10, 21));

        ModPacket packet = GetPacket();
        packet.Write(d.X);
        packet.Write(d.Y);
        packet.Send();
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI) {
    }
}