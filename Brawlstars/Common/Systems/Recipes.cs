using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Common.Systems;

public class Recipes : ModSystem {
    public override void AddRecipes() {
        // 根据官方商店中超低的汇率，1个宝石在优惠前只价值9金币。
        Recipe.Create(ItemID.Emerald)
            .AddIngredient(ItemID.GoldCoin, 9)
            .Register();

        // 2022/01/22 2 USD for 140 mega boxes
        Recipe.Create(ItemID.GoldChest, 140)
            .AddIngredient(ItemID.Emerald, 30)
            .Register();
    }
}