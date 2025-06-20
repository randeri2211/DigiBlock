using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;
using System.Collections.Generic;
using DigiBlock.Content.Damage;

namespace DigiBlock.Content.Items.Accessories
{
    public class AllSpecials : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DigiBlockPlayer>().digimonAllSpecialAbilities = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "AllSpecial", "Allows the digimon to use all special abilities"));
        }
    }
}
