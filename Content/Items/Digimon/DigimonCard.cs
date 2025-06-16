using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.ID;
using DigiBlock.Common;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using DigiBlock.Content.Systems;
using System;


namespace DigiBlock.Content.Items.Digimon
{
    public class DigimonCard : ModItem
    {
        public DigimonBase digimon;
        public Digivice digivice;
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
            Item.width = 17;
            Item.height = 23;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Blue;
            if (_digimonNpcType > 0 && digimon == null)
            {
                setDigimonNpcType(_digimonNpcType); // Will call TryInitializeDigimon
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (digimon != null)
            {
                Mod mod = ModContent.GetInstance<DigiBlock>();
                Player player = Main.LocalPlayer;
                // mod.Logger.Info(digimon.name + "," + digimon.Name);
                tooltips.Add(new TooltipLine(Mod, "DigimonName", "Name: " + digimon.name));
                tooltips.Add(new TooltipLine(Mod, "DigimonLevel", "Level: " + digimon.level));
                tooltips.Add(new TooltipLine(Mod, "DigimonHP", "HP: " + digimon.NPC.life + "/" + digimon.NPC.lifeMax));
                tooltips.Add(new TooltipLine(Mod, "DigimonEXP", "Exp: " + digimon.getEXP() + "/" + digimon.maxEXP));
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage", "Contact Digital Damage: " + digimon.CalculateDamage(digimon.physicalDamage)));
                tooltips.Add(new TooltipLine(Mod, "DigimonDamage2", "Special Digital Damage: " + digimon.CalculateDamage(digimon.specialDamage)));
                tooltips.Add(new TooltipLine(Mod, "DigimonAgility", "Agility: " + digimon.agility));
                tooltips.Add(new TooltipLine(Mod, "DigimonDefense", "Defense: " + digimon.NPC.defense));
                tooltips.Add(new TooltipLine(Mod, "DigimonType", "Digimon Type: " + digimon.Name));
                TooltipLine attributeTooltip = new TooltipLine(Mod, "DigimonAttribute", "Digimon Attribute: " + digimon.attribute);
                switch (digimon.attribute)
                {
                    case Attributes.Data:
                        attributeTooltip.OverrideColor = Color.Blue;
                        break;
                    case Attributes.Virus:
                        attributeTooltip.OverrideColor = Color.Red;
                        break;
                    case Attributes.Vaccine:
                        attributeTooltip.OverrideColor = Color.Green;
                        break;
                }
                tooltips.Add(attributeTooltip);
            }
        }

        public override void SaveData(Terraria.ModLoader.IO.TagCompound tag)
        {
            tag["npcType"] = getDigimonNpcType();
            tag["contactDamage"] = digimon.physicalDamage;
            tag["specialDamage"] = digimon.specialDamage;
            tag["agility"] = digimon.agility;
            tag["level"] = digimon.level;
            tag["exp"] = digimon.getEXP();
            tag["currentHP"] = digimon.NPC.life;
            tag["maxHP"] = digimon.maxHP;
            tag["defense"] = digimon.defense;
            tag["specialIndex"] = digimon.specialAbilityIndex;
            var biomeKillTag = new TagCompound();

            foreach (var pair in digimon.biomeKills)
            {
                biomeKillTag[$"{pair.Key}"] = pair.Value;
            }

            tag["biomeKills"] = biomeKillTag;
        }

        public override void LoadData(Terraria.ModLoader.IO.TagCompound tag)
        {
            if (tag.ContainsKey("npcType"))
            {
                setDigimonNpcType(tag.GetInt("npcType"));
            }
            if (tag.ContainsKey("contactDamage"))
            {
                digimon.physicalDamage = tag.GetInt("contactDamage");
            }
            if (tag.ContainsKey("specialDamage"))
            {
                digimon.specialDamage = tag.GetInt("specialDamage");
            }
            if (tag.ContainsKey("agility"))
            {
                digimon.agility = tag.GetInt("agility");
            }
            if (tag.ContainsKey("level"))
            {
                digimon.level = tag.GetInt("level");
                digimon.maxEXP = DigiblockConstants.StartingEXP;
                for (int i = 1; i < digimon.level; i++)
                {
                    digimon.maxEXP = (int)(digimon.maxEXP * DigiblockConstants.LevelingEXPMultiplier);
                }
            }
            if (tag.ContainsKey("exp"))
            {
                digimon.GiveEXP(tag.GetInt("exp"));
            }
            if (tag.ContainsKey("currentHP"))
            {
                digimon.NPC.life = tag.GetInt("currentHP");
            }
            if (tag.ContainsKey("maxHP"))
            {
                digimon.maxHP = tag.GetInt("maxHP");
            }
            if (tag.ContainsKey("defense"))
            {
                digimon.defense = tag.GetInt("defense");
            }
            if (tag.ContainsKey("specialIndex"))
            {
                digimon.specialAbilityIndex = tag.GetInt("specialIndex");
            }
            digimon.biomeKills = new Dictionary<DigimonSpawnBiome, int>();

            if (tag.TryGet("biomeKills", out TagCompound biomeKillTag))
            {
                foreach (var pair in biomeKillTag)
                {
                    if (Enum.TryParse(pair.Key, out DigimonSpawnBiome biome))
                    {
                        digimon.biomeKills[biome] = biomeKillTag.GetInt(pair.Key);
                    }
                }
            }
            digimon.CalculateStats();
        }

        public int TryInitializeDigimon()
        {
            if (_digimonNpcType <= 0)
                return -1;

            // Kill the swap digimon
            if (digimon != null)
            {
                NPC npc = digimon.NPC;
                npc.StrikeInstantKill();
                npc.life = 0;
                npc.active = false;
                npc.netSkip = -1;
                npc.netUpdate = true;
            }
            int npcIndex = -1;
            if (digivice != null)
            {
                Player player = digivice.FindPlayer();
                npcIndex = NPC.NewNPC(null, (int)player.Center.X, (int)player.Center.Y, _digimonNpcType);
                digimon = Main.npc[npcIndex].ModNPC as DigimonBase;
                digimon.card = this;
                digimon.NPC.friendly = true;
                digimon.NPC.active = true;
                digimon.playerOwner = player;
                digimon.CalculateStats();
            }
            else
            {
                npcIndex = NPC.NewNPC(null, 0, 0, _digimonNpcType);
                digimon = Main.npc[npcIndex].ModNPC as DigimonBase;
                digimon.card = this;
                digimon.NPC.friendly = true;
                digimon.NPC.active = false;
                digimon.playerOwner = null;
            }
            return npcIndex;
        }
    }
}
