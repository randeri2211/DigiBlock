using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.ModLoader.IO;
using DigiBlock.Content.UI;
using System.Collections.Generic;


namespace DigiBlock.Content.Items.Digimon
{
    public class Digivice : ModItem
    {
        public Item card = new Item();
        public Item disk = new Item();
        private string card_tag = "item";
        private string diskTag = "disk";

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = Item.useTime;
            Item.useTurn = true;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;


            card = new Item(ItemID.None);
        }

        // Creating item craft
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // TODO: Come up with a recipe
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
            if (!card.IsAir)
            {
                tag[card_tag] = ItemIO.Save(card);
                if (!disk.IsAir)
                {
                    tag[diskTag] = ItemIO.Save(disk);
                }
            }
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey(card_tag))
            {
                card = ItemIO.Load(tag.GetCompound(card_tag));
                if (tag.ContainsKey(diskTag))
                {
                    disk = ItemIO.Load(tag.GetCompound(diskTag));
                }
            }
            else
            {
                card = new Item();
                card.SetDefaults(0);
                disk = new Item();
                disk.SetDefaults(0);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            var digimonData = card.Name;

            var line = new TooltipLine(Mod, "Digivice Digimon", digimonData);
            tooltips.Add(line);
        }

        public bool IsItemStillAccessible()
        {
            // Check player inventories
            foreach (Player player in Main.player)
            {
                if (!player.active) continue;
                foreach (Item invItem in player.inventory)
                {
                    if (invItem == Item || (invItem.ModItem != null && invItem.ModItem == Item.ModItem))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        public Player FindPlayer()
        {
            // Check players
            foreach (Player player in Main.player)
            {
                if (!player.active) continue;
                foreach (Item invItem in player.inventory)
                {
                    if (invItem.ModItem == this)
                        return player;
                }
            }

            return null;
        }
    }
}