using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System;
using Terraria.GameContent.UI;
using Terraria.ID;


namespace DigiBlock.Content.Items.Digimon
{
    public class DigimonCard : ModItem
    {
        public DigimonBase digimon;
        private int _digimonNpcType = -1;
        public int getDigimonNpcType()
        {
            return _digimonNpcType;
        }
        public int setDigimonNpcType(int digimon, bool init = true)
        {
            _digimonNpcType = digimon;
            if (init)
            {
                return TryInitializeDigimon();
            }
            return -1;
        }

        public override void SetDefaults()
        {
            // // Player player = Main.LocalPlayer;
            // // int npcIndex = NPC.NewNPC(null, (int)(player.Center.X), (int)(player.Center.Y), ModContent.NPCType<Agumon>()); // spawn off-screen
            // // digimon = Main.npc[npcIndex].ModNPC as Agumon;
            // if (digimonNpcType <= 0)
            // {
            //     Console.WriteLine("Digimon Typex:" + digimonNpcType);
            //     digimonNpcType = ModContent.NPCType<Agumon>(); // fallback
            //     Console.WriteLine("Digimon Typex after:" + digimonNpcType);
            // }
            // else
            // {
            //     Player player = Main.LocalPlayer;
            //     int npcIndex = NPC.NewNPC(null, (int)player.Center.X, (int)player.Center.Y, digimonNpcType);
            //     Console.WriteLine("Digimon Type:" + Main.npc[npcIndex].FullName);
            //     digimon = Main.npc[npcIndex].ModNPC as DigimonBase;
            //     digimon.card = this;
            //     digimon.NPC.friendly = true;
            //     digimon.NPC.active = false;
            // }
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
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
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage", "Digital Damage: " + digimon.baseDmg)); //TODO:Change for an ability damage
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage2", "Contact Damage: " + digimon.NPC.damage));
                tooltips.Add(new TooltipLine(Mod, "DigimonAgility", "Contact Damage: " + digimon.agility));
            }
            tooltips.Add(new TooltipLine(Mod, "Type", "Type: " + getDigimonNpcType()));
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            // recipe.AddIngredient<SteelShard>(7); // We are using custom material for the craft, 7 Steel Shards
            // recipe.AddIngredient(ItemID.Wood, 3); // Also, we are using vanilla material to craft, 3 Wood
            // recipe.AddTile(TileID.Anvils); // Crafting station we need for craft, WorkBenches, Anvils etc. You can find them here - https://terraria.wiki.gg/wiki/Tile_IDs
            recipe.Register();
        }

        public override void SaveData(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag["npcType"] = getDigimonNpcType();
        }

        public override void LoadData(Terraria.ModLoader.IO.TagCompound tag)
        {
            if (tag.ContainsKey("npcType"))
                setDigimonNpcType(tag.GetInt("npcType"));
        }

        private int TryInitializeDigimon()
        {
            if (_digimonNpcType <= 0)
                return -1;
            if (digimon != null)
            {
                NPC npc = digimon.NPC;
                npc.StrikeInstantKill();
                npc.life = 0;
                npc.active = false;
                npc.netSkip = -1;
                npc.netUpdate = true;
            }
            Player player = Main.LocalPlayer;

            int npcIndex = NPC.NewNPC(null, (int)player.Center.X, (int)player.Center.Y, _digimonNpcType);
            digimon = Main.npc[npcIndex].ModNPC as DigimonBase;

            digimon.card = this;
            digimon.NPC.friendly = true;
            digimon.NPC.active = false;
            return npcIndex;
        }
    }
}
