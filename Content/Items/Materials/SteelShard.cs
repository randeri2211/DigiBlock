// What libraries we use in the code
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using Terraria;

namespace DigiBlock.Content.Items.Materials // Where your code located
{
    public class SteelShard : ModItem // Your item name (SteelShard) and type (ModItem)
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100; // How many items need for research in Journey Mode
        }

        public override void SetDefaults()
        {
            Item.width = 22; // Width of an item sprite
            Item.height = 24; // Height of an item sprite
            Item.maxStack = 9999; // How many items can be in one inventory slot
            Item.value = 100; // Item sell price in copper coins
            Item.rare = ItemRarityID.Blue; // The color of item's name in game. Check https://terraria.wiki.gg/wiki/Rarity
        }



    }
}