using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using System;
using Terraria.GameContent.UI;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DigiBlock.Common;


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
                tooltips.Add(new TooltipLine(Mod, "DigimonLevel", "Level: " + digimon.level));
                tooltips.Add(new TooltipLine(Mod, "DigimonHP", "HP: " + digimon.NPC.life + "/" + digimon.NPC.lifeMax));
                tooltips.Add(new TooltipLine(Mod, "DigimonEXP", "Exp: " + digimon.getEXP() + "/" + digimon.maxEXP));
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage", "Digital Damage: " + digimon.baseDmg)); //TODO:Change for an ability damage
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage2", "Contact Damage: " + digimon.NPC.damage));
                tooltips.Add(new TooltipLine(Mod, "DigimonAgility", "Agility: " + digimon.agility));
                tooltips.Add(new TooltipLine(Mod, "DigimonType", "Type: " + digimon.Name));
            }
            tooltips.Add(new TooltipLine(Mod, "Type", "Type: " + getDigimonNpcType()));
        }

        public override void SaveData(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag["npcType"] = getDigimonNpcType();
            tag["baseDmg"] = digimon.baseDmg;
            tag["agility"] = digimon.agility;
            tag["level"] = digimon.level;
            tag["exp"] = digimon.getEXP();
        }

        public override void LoadData(Terraria.ModLoader.IO.TagCompound tag)
        {
            if (tag.ContainsKey("npcType"))
            {
                setDigimonNpcType(tag.GetInt("npcType"));
            }
            if (tag.ContainsKey("baseDmg"))
            {
                digimon.baseDmg = tag.GetInt("baseDmg");
            }
            if (tag.ContainsKey("agility"))
            {
                digimon.agility = tag.GetInt("agility");
            }
            if (tag.ContainsKey("level"))
            {
                digimon.level = tag.GetInt("level");
                digimon.maxEXP = DigiblockConstants.StartingEXP;
                for (int i = 0; i < digimon.level; i++)
                {
                    digimon.maxEXP = (int)(digimon.maxEXP * DigiblockConstants.LevelingEXPMultiplier);
                }
            }
            if (tag.ContainsKey("exp"))
            { 
                digimon.GiveEXP(tag.GetInt("exp"));
            }
            
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

        public Vector2? FindItemLocation()
        {
            // Check players
            foreach (Player player in Main.player)
            {
                if (!player.active) continue;
                foreach (Item invItem in player.inventory)
                {
                    if (invItem.ModItem == this)
                        return player.Center;
                }
            }
            Console.WriteLine("Checked Players");
            // Check chests
            for (int i = 0; i < Main.chest.Length; i++)
            {
                Chest chest = Main.chest[i];
                if (chest == null) continue;

                foreach (Item chestItem in chest.item)
                {
                    if (chestItem.ModItem == this)
                        return new Point(chest.x, chest.y).ToWorldCoordinates();
                }
            }
            Console.WriteLine("Checked Chests");

            return null;
        }
    }
}
