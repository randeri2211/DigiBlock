using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text.Json; // or Newtonsoft.Json
using Microsoft.Xna.Framework;
using DigiBlock.Common;

namespace DigiBlock.Content.Systems
{
    public class EvolutionSystem : ModSystem
    {
        JsonDocument evolutions;
        public override void OnModLoad()
        {
            // Open the csv file for all the evolutions
            Mod mod = ModContent.GetInstance<DigiBlock>();
            string path = "Content/Data/Evolutions.json";
            try
            {
                byte[] data = mod.GetFileBytes(path);
                string csvContent = Encoding.UTF8.GetString(data);
                evolutions = JsonDocument.Parse(csvContent);
            }
            catch (Exception ex)
            {
                mod.Logger.Error($"[JSON ERROR] {ex.Message}");
            }
        }

        public override void PreUpdateNPCs()
        {
            Mod mod = ModContent.GetInstance<DigiBlock>();
            int count = 0;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].ModNPC is DigimonBase digimon)
                {
                    if (digimon.NPC.friendly)
                    {
                        count += 1;
                        string[] fullName = digimon.GetType().ToString().Split('.');
                        string digimonName = fullName[fullName.Length - 1];
                        // Console.WriteLine($"{digimonName} {digimon.NPC.life}");
                        if (evolutions.RootElement.TryGetProperty(digimonName, out JsonElement digivolutions))
                        {
                            foreach (var digiv in digivolutions.EnumerateArray())
                            {
                                JsonElement conditions = digiv.GetProperty("Conditions");
                                string evolutionName = digiv.GetProperty("Evolution").GetString();

                                if (CanEvolve(digimon, conditions))
                                {
                                    mod.Logger.Info("can evolve");
                                    Evolve(digimon, evolutionName);
                                }
                            }
                        }
                    }
                }
            }
            // mod.Logger.Info("count: " + count);
        }

        public bool CanEvolve(DigimonBase digimon, JsonElement conditions)
        {
            if (digimon.justEvolved)
            {
                return false;
            }
            List<bool> conditionChecks = new List<bool>();
            if (conditions.TryGetProperty("Level", out JsonElement levelCon))
            {
                int level = levelCon.GetInt32();
                conditionChecks.Add(digimon.level >= level);
            }

            // Making sure all the checks are true for this evolution
            bool overallChecks = true;
            foreach (bool check in conditionChecks)
            {
                overallChecks = overallChecks && check;
            }

            return overallChecks;
        }

        public void Evolve(DigimonBase digimon, string evolutionName)
        {
            Mod mod = ModContent.GetInstance<DigiBlock>();
            NPC npc = digimon.NPC;
            if (mod.Find<ModNPC>(evolutionName) is DigimonBase evolved)
            {
                int etype = evolved.Type;
                Vector2 pos = npc.Center;
                int newNpcId = NPC.NewNPC(null, (int)pos.X, (int)pos.Y, etype);
                if (Main.npc[newNpcId].ModNPC is DigimonBase evolvedNPC && evolvedNPC.NPC.active)
                {
                    mod.Logger.Debug("Damage before " + digimon.NPC.damage);
                    mod.Logger.Debug("dd " + digimon.card.digimon.name);
                    digimon.card.digimon = evolvedNPC;
                    digimon.card.setDigimonNpcType(etype, false);
                    evolvedNPC.copyData(digimon);
                    evolvedNPC.NPC.lifeMax += DigiblockConstants.EvolutionBonus;
                    evolvedNPC.NPC.damage += DigiblockConstants.EvolutionBonus;
                    evolvedNPC.contactDamage += DigiblockConstants.EvolutionBonus;
                    mod.Logger.Debug("Damage after " + evolvedNPC.NPC.damage);
                    mod.Logger.Debug("Damage after " + evolvedNPC.contactDamage);
                    mod.Logger.Debug("ff " + digimon.card.digimon.name);
                }
            }
            digimon.justEvolved = true;

            npc.StrikeInstantKill();
            npc.life = 0;
            npc.active = false;
            npc.netSkip = -1;
            npc.netUpdate = true;
            
            mod.Logger.Info("Done Evolving");
            CombatText.NewText(npc.Hitbox, Color.Orange, $"{evolutionName}!"+npc.active+npc.life);
        }
    }
}
