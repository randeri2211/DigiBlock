using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;
using System.Collections.Generic;

namespace DigiBlock.Content.Items.Accessories
{
    public class EXPChip : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("EXP Chip");
            Item.accessory = true;
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Access the custom ModPlayer and modify stats
            player.GetModPlayer<DigiBlockPlayer>().digimonEXPPercent += 0.1f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "EXPChip", "Increases your Digimon EXP gain by 10%"));
        }
    }
}
