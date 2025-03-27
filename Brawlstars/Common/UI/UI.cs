#nullable enable
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace brawlstars.Brawlstars.Common.UI;

public class Ui : ModSystem {
    private BrawlerStateGroupUi? _brawlerStateGroupUi;

    private GameTime _lastUpdateUiGameTime;
    private UserInterface? _userInterface;

    public override void Load() {
        if (!Main.dedServ) {
            _userInterface = new UserInterface();
            _brawlerStateGroupUi = new BrawlerStateGroupUi();
            _brawlerStateGroupUi.Activate();
        }
    }

    public override void Unload() {
        _brawlerStateGroupUi?.Deactivate();
        _brawlerStateGroupUi = null;
    }

    public override void UpdateUI(GameTime gameTime) {
        _lastUpdateUiGameTime = gameTime;
        if (_userInterface?.CurrentState != null) _userInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        if (mouseTextIndex != -1)
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "MyMod: MyInterface",
                delegate {
                    if (_lastUpdateUiGameTime != null && _userInterface?.CurrentState != null)
                        _userInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
    }

    public void Show(BrawlerItem brawlerItem) {
        _brawlerStateGroupUi?.Show(brawlerItem);
        _userInterface?.SetState(_brawlerStateGroupUi);
    }

    public void Hide() {
        _brawlerStateGroupUi?.Hide();
        _userInterface?.SetState(null);
    }
}