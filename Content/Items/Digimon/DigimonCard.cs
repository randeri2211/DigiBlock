using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using System;
using DigiBlock.Content.Damage;

namespace DigiBlock.Content.Items.Digimon
{
    public class DigimonCard : ModItem
    {
        public DigimonBase digimon;
        public override void SetDefaults()
        {
            Item.DamageType = ModContent.GetInstance<DigitalDamage>();
            Item.damage = 5;
            Player player = Main.LocalPlayer;
            int npcIndex = NPC.NewNPC(null, (int)(player.Center.X), (int)(player.Center.Y), ModContent.NPCType<Agumon>()); // spawn off-screen
            digimon = Main.npc[npcIndex].ModNPC as Agumon;
            digimon.card = this;
            digimon.NPC.friendly = true;
            digimon.NPC.active = false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // TODO:Add more digimon data on display
            if (digimon != null)
            {
                Mod mod = ModContent.GetInstance<DigiBlock>();
                Player player = Main.LocalPlayer;
                // mod.Logger.Info(digimon.name + "," + digimon.Name);
                tooltips.Add(new TooltipLine(Mod, "DigimonName", "Name: " + digimon.name));
                tooltips.Add(new TooltipLine(Mod, "DigimonType", "Type: " + digimon.Name));
                tooltips.Add(new TooltipLine(Mod, "DigimonHP", "HP: " + digimon.NPC.life + "/" + digimon.NPC.lifeMax));
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage", "Digital Damage: " + digimon.baseDmg));
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage2", "Contact Damage: " + digimon.NPC.damage));
                tooltips.Add(new TooltipLine(Mod, "DigimonActive", "Active: " + digimon.NPC.active));
                tooltips.Add(new TooltipLine(Mod, "Digimonpos", "Pos: " + digimon.NPC.Center.X + "," + digimon.NPC.Center.Y));
                tooltips.Add(new TooltipLine(Mod, "Digimondir", "Dir: " + digimon.NPC.direction));
            }
        }
        
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient<SteelShard>(7); // We are using custom material for the craft, 7 Steel Shards
            // recipe.AddIngredient(ItemID.Wood, 3); // Also, we are using vanilla material to craft, 3 Wood
            // recipe.AddTile(TileID.Anvils); // Crafting station we need for craft, WorkBenches, Anvils etc. You can find them here - https://terraria.wiki.gg/wiki/Tile_IDs
            recipe.Register();
        }
    }
}
