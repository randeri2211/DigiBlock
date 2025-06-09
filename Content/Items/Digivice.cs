using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.IO;
using DigiBlock.Content.UI;
using System.Collections.Generic;
using System.Linq;
using Terraria.UI;


namespace DigiBlock.Content.Items // Where is your code locates
{
    public class Digivice : ModItem
    {
        // TODO: Needs to hold a digimon item
        public Item item = new Item();
        private String item_tag = "item";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; // How many items need for research in Journey Mode
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useTurn = true;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            

            item = new Item(ItemID.None);
        }

        // Creating item craft
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient<SteelShard>(7); // We are using custom material for the craft, 7 Steel Shards
            // recipe.AddIngredient(ItemID.Wood, 3); // Also, we are using vanilla material to craft, 3 Wood
            // recipe.AddTile(TileID.Anvils); // Crafting station we need for craft, WorkBenches, Anvils etc. You can find them here - https://terraria.wiki.gg/wiki/Tile_IDs
            recipe.Register();
        }

        public override bool? UseItem(Player player)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                ModContent.GetInstance<DigiviceUISystem>().ToggleUI(this);
            }
            return true;
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void SaveData(TagCompound tag)
        {
            if (!item.IsAir)
                tag[item_tag] = ItemIO.Save(item);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(item_tag))
                item = ItemIO.Load(tag.GetCompound(item_tag));
            else
            {
                item = new Item();
                item.SetDefaults(0);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            var digimonData = item.Name;
            // TODO:Add more digimon data on display

            var line = new TooltipLine(Mod, "Digivice Digimon", digimonData);
            tooltips.Add(line);
        }
    }
}