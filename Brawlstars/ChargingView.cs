#nullable enable
using System;
using System.Linq;
using brawlstars.Brawlstars.BrawlerContent.ChargingUi;
using brawlstars.Brawlstars.Common.Configs;
using brawlstars.Brawlstars.Common.UI;
using brawlstars.Brawlstars.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI.Chat;

namespace brawlstars.Brawlstars;

public abstract class ChargingView(BrawlerBehavior behavior)
    : IChargingProgress, IBrawlerAbility, IBrawlerStateUiDelegate {
    public readonly BrawlerBehavior Behavior = behavior;

    public event Func<bool>? IsUpdatableFilters;

    private int _cooldown;

    /// <summary>
    /// 剩余冷却时间，在归0之前不进行更新，只有在<see cref="MaxCooldown"/>大于0时才有效。
    /// </summary>
    public int Cooldown {
        get => _cooldown;
        set {
            _cooldown = value;
            if (_cooldown < 0) {
                _cooldown = 0;
            }
        }
    }

    // todo: 包装冷却时间对象，方便安全执行每个冷却(惩罚)。
    public int MaxCooldown;

    public void SetCooldownToMax() {
        Cooldown = MaxCooldown;
    }

    public bool HasCooldown => Cooldown > 0 && MaxCooldown > 0;

    public int CooldownValue => Cooldown * 100 / MaxCooldown;

    public virtual void Update() {
        if (
            IsUpdatableFilters == null ||
            IsUpdatableFilters.GetInvocationList().Cast<Func<bool>>().All(handler => handler())
        ) {
            if (Cooldown == 0) {
                ActualUpdate();
            }
        }

        Using = false;
        if (HasCooldown) {
            Cooldown--;
        }
    }

    /**
     * 获取下一个。
     */
    public virtual bool Next() {
        if (ServerConfig.ServerDebugMode || (Usable && ActualNext())) {
            Reset();
            Using = true;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 重置充能。
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// 更新，在此物品上每帧调用。
    /// </summary>
    protected abstract void ActualUpdate();

    /// <summary>
    /// 得到下一个，意为消耗全部充能以指示操作得以支持。
    /// </summary>
    /// <returns></returns>
    protected abstract bool ActualNext();

    public abstract int Value { get; }

    public bool Using { get; private set; }
    public bool Usable { get; set; } = true;

    public abstract Color ThemeColor { get; }
    public abstract string Tip { get; }
    public abstract string TipColor { get; }


    protected UIImage UiImageCharging;
    protected UIImage UiImageChargingBorder;
    protected UIText UiText;
    private string LastText = string.Empty;

    protected static Asset<Texture2D> ChargingBorderTexture =>
        ChargingUiAssets.Instance.RequestTextureAsset("Charging_Border");

    protected Asset<Texture2D> ChargingTexture => GetChargingTexture(TextureIndex(Value));

    protected Asset<Texture2D> TimeLeftChargingTexture => GetChargingTexture(TextureIndex(CooldownValue));


    protected int TextureIndex(float v) {
        return Math.Min(v switch {
            >= 100 => 18,
            <= 0 => 0,
            _ => (int)(v * 17f / 100f) + 1
        }, 18);
    }

    protected static Asset<Texture2D> GetChargingTexture(int textureIndex) {
        return ChargingUiAssets.Instance.RequestTextureAsset("Charging_" + textureIndex);
    }

    /// <summary>
    /// 先加载所有纹理。。
    /// </summary>
    protected static void LoadAllTextures() {
        _ = ChargingBorderTexture;
        for (var i = 0; i < 18; i++) {
            _ = GetChargingTexture(i);
        }
    }

    private bool _initialized = false;

    public void Init() {
        UiImageCharging = new(Asset<Texture2D>.Empty);
        UiImageChargingBorder = new(ChargingBorderTexture);
        UiText = new(string.Empty);
    }

    public virtual void Show(BrawlerStateUiContainer ui) {
        if (!_initialized) {
            _initialized = true;
            Init();
        }

        if (Tip.Length == 0) {
            return;
        }

        LoadAllTextures();

        UiImageCharging.Width.Set(112, 0);
        UiImageCharging.Height.Set(24, 0);
        UiImageCharging.Top.Set(29, 0);
        ui.Append(UiImageCharging);

        UiImageChargingBorder.Width.Set(112, 0);
        UiImageChargingBorder.Height.Set(24, 0);
        UiImageChargingBorder.Top.Set(29, 0);
        ui.Append(UiImageChargingBorder);

        UiText.Width.Set(112, 0);
        UiText.Height.Set(24, 0);
        UiText.Top.Set(10, 0);
        ui.Append(UiText);
    }

    public virtual void BeforeDraw(BrawlerStateUiContainer ui, SpriteBatch spriteBatch) {
        if (HasCooldown) {
            UiImageCharging.SetImage(TimeLeftChargingTexture);
            UiImageCharging.Color = Color.White;
        } else {
            UiImageCharging.SetImage(ChargingTexture);
            UiImageCharging.Color = ThemeColor;
        }

        if (!Usable) {
            UiImageCharging.Color = Color.LightGray;
        }

        UiImageChargingBorder.Color = Color.Black;

        var colorTip = TipColor.ColorText(Tip);
        if (!LastText.Equals(Tip)) {
            LastText = Tip;
            var length = ChatManager.GetStringSize(FontAssets.MouseText.Value, Tip, new Vector2(1)).X;
            var scale = length > 114f ? 114f / length : 1f;
            UiText.Remove();
            UiText = new UIText(colorTip, scale);
            UiText.Width.Set(112, 0);
            UiText.Height.Set(24, 0);
            UiText.Top.Set(10 + 7 * (1 - scale), 0);
            UiText.ShadowColor = Color.Black;
            ui.Append(UiText);
        }

        if (UiImageChargingBorder.IsMouseHovering || UiImageCharging.IsMouseHovering) {
            Main.hoverItemName = $"{(HasCooldown ? -CooldownValue : Value)} / {colorTip}";
        }
    }

    public virtual void Hide(BrawlerStateUiContainer ui) {
        UiImageCharging.Remove();
        UiImageChargingBorder.Remove();
        UiText.Remove();
    }
}