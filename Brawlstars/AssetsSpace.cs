using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;

namespace brawlstars.Brawlstars;

public class AssetsSpace {
    private readonly string _assetPathFill;
    private readonly string _assetPath;

    public AssetsSpace() {
        _assetPathFill = GetType().Namespace?.Replace('.', '/') + "/";
        _assetPath = _assetPathFill.Substring(11); // minus "brawlstars/"
    }

    public string AssetFile(string assetName) {
        return _assetPathFill + assetName;
    }

    public string TextureFile(string name) {
        return AssetFile("Texture/" + name);
    }

    public string SoundFile(string name) {
        return AssetFile("Sound/" + name);
    }

    public T Request<T>(string assetName) where T : class {
        return RequestAsset<T>(assetName).Value;
    }

    public Asset<T> RequestAsset<T>(string assetName) where T : class {
        return BrawlStars.Instance.Assets.Request<T>(
            _assetPath + assetName
        );
    }

    public Texture2D RequestTexture(string name) {
        return RequestTextureAsset(name).Value;
    }

    public Asset<Texture2D> RequestTextureAsset(string name) {
        return RequestAsset<Texture2D>("Texture/" + name);
    }

    public SoundStyle RequestSound(string name) {
        return new SoundStyle(_assetPathFill + "Sound/" + name) {
            Volume = 0.5f,
            MaxInstances = 0
        };
    }
}