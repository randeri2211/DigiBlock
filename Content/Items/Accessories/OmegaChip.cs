using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Content.Systems;
using System.Collections.Generic;
using DigiBlock.Content.Damage;

namespace DigiBlock.Content.Items.Accessories
{
    public class OmegaChip : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 31;
            Item.height = 31;
            Item.value = Item.sellPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<DigitalDamage>() *= 1.1f;
            player.GetModPlayer<DigiBlockPlayer>().digimonMaxHPPercent += 0.1f;
            player.GetModPlayer<DigiBlockPlayer>().digimonDefensePercent += 0.1f;
            player.GetModPlayer<DigiBlockPlayer>().digimonEXPPercent += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe= CreateRecipe();
            recipe.AddIngredient<EXPChip>();
            recipe.AddIngredient<DefenseChip>();
            recipe.AddIngredient<DamageChip>();
            recipe.AddIngredient<HPChip>();
            recipe.Register();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "DamageChip", "Increases your Digimon Damage by 10%"));
            tooltips.Add(new TooltipLine(Mod, "EXPChip", "Increases your Digimon EXP gain by 10%"));
            tooltips.Add(new TooltipLine(Mod, "HPChip", "Increases your Digimon MaxHP by 10%"));
            tooltips.Add(new TooltipLine(Mod, "DefenseChip", "Increases your Digimon Defense by 10%"));
        }
    }
}
