using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon;
using System.Text;
using System;
using System.Collections.Generic;
using System.Text.Json; 
using Microsoft.Xna.Framework;
using DigiBlock.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;


namespace DigiBlock.Content.Systems
{
    public class EvolutionSystem : ModSystem
    {
        public JsonDocument evolutions;
        private List<EvolutionEffect> activeAnimations = new();
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
            // Evolution Loop
            CheckEvolutions();


            // Animation Loop
            UpdateAnimations();
        }

        private void CheckEvolutions()
        {
            Mod mod = ModContent.GetInstance<DigiBlock>();
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].ModNPC is DigimonBase digimon)
                {
                    if (digimon.evolving || !digimon.NPC.active)
                    {
                        continue;
                    }
                    if (digimon.NPC.friendly)
                    {
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
                                    TriggerEvolution(digimon, evolutionName);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void UpdateAnimations()
        {
            for (int i = 0; i < activeAnimations.Count; i++)
            {
                var effect = activeAnimations[i];
                var digimon = effect.Digimon;

                if (!digimon.NPC.active)
                {
                    activeAnimations.RemoveAt(i);
                    continue;
                }

                effect.Timer++;
                if (effect.Timer >= 2) // delay in ticks
                {
                    effect.Timer = 0;

                    if (effect.CurrentRowBase < effect.RowColorsBase.Length)
                    {
                        float yOffset = ((float)effect.CurrentRowBase / effect.RowColorsBase.Length) * digimon.NPC.height;
                        float worldY = digimon.NPC.Top.Y + yOffset;
                        float worldX = digimon.NPC.Left.X + Main.rand.NextFloat(digimon.NPC.width);
                        Vector2 position = new Vector2(worldX, worldY);

                        Dust.NewDustPerfect(position, DustID.Smoke, Vector2.Zero, 0, effect.RowColorsBase[effect.CurrentRowBase], 1f);

                        effect.CurrentRowBase++;
                    } else if (effect.CurrentRowEvolved < effect.RowColorsEvolved.Length) {
                        float yOffset = ((float)effect.CurrentRowEvolved / effect.RowColorsEvolved.Length) * effect.EvolutionInstance.NPC.height;
                        float worldY = digimon.NPC.Bottom.Y - yOffset;
                        float worldX = digimon.NPC.Left.X + Main.rand.NextFloat(digimon.NPC.width);
                        Vector2 position = new Vector2(worldX, worldY);

                        Dust.NewDustPerfect(position, DustID.Smoke, Vector2.Zero, 0, effect.RowColorsEvolved[effect.CurrentRowEvolved], 1f);

                        effect.CurrentRowEvolved++;
                    }
                    else
                    {
                        digimon.evolving = false;
                        Evolve(digimon, effect.EvolutionInstance);
                        activeAnimations.RemoveAt(i); // animation complete
                    }
                }
            }
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

        public void TriggerEvolution(DigimonBase digimon, string evolutionName)
        {
            digimon.evolving = true;
            Mod mod = ModContent.GetInstance<DigiBlock>();
            if (mod.Find<ModNPC>(evolutionName) is DigimonBase evolved)
            {
                EvolutionAnimation(digimon, evolved);
            }
        }

        public void Evolve(DigimonBase digimon, DigimonBase evolvedInstance)
        {
            digimon.evolving = true;
            Mod mod = ModContent.GetInstance<DigiBlock>();
            NPC npc = digimon.NPC;

            int etype = evolvedInstance.Type;

            Vector2 pos = npc.Center;
            int newNpcId = NPC.NewNPC(null, (int)pos.X, (int)pos.Y, etype);
            if (Main.npc[newNpcId].ModNPC is DigimonBase evolvedNPC && evolvedNPC.NPC.active)
            {
                mod.Logger.Debug("Evolving " + digimon.card.digimon.name);
                digimon.card.digimon = evolvedNPC;
                digimon.card.setDigimonNpcType(etype, false);
                evolvedNPC.copyData(digimon);
                evolvedNPC.maxHP += DigiblockConstants.EvolutionBonus;
                evolvedNPC.contactDamage += DigiblockConstants.EvolutionBonus;
                evolvedNPC.specialDamage += DigiblockConstants.EvolutionBonus;
                evolvedNPC.defense += DigiblockConstants.EvolutionBonus;
                evolvedNPC.CalculateStats();
                mod.Logger.Debug("Into " + digimon.card.digimon.name);
            }
            digimon.justEvolved = true;

            npc.StrikeInstantKill();
            npc.life = 0;
            npc.active = false;
            npc.netSkip = -1;
            npc.netUpdate = true;

            mod.Logger.Info("Done Evolving");
            CombatText.NewText(npc.Hitbox, Color.Orange, $"{digimon.Name}!" + npc.active + npc.life);
        }

        public void EvolutionAnimation(DigimonBase digimon, DigimonBase evolutionInstance)
        {
            Texture2D textureBase = ModContent.Request<Texture2D>(digimon.NPC.ModNPC.Texture).Value;
            Texture2D textureEvolved = ModContent.Request<Texture2D>(evolutionInstance.NPC.ModNPC.Texture).Value;

            activeAnimations.Add(new EvolutionEffect
            {
                Digimon = digimon,
                EvolutionInstance = evolutionInstance,
                RowColorsBase = GetAverageColorPerRow(textureBase),
                RowColorsEvolved = GetAverageColorPerRow(textureEvolved),
                CurrentRowBase = 0,
                CurrentRowEvolved = 0,
                Timer = 0
            });
        }

        public static Color[] GetAverageColorPerRow(Texture2D texture)
        {
            int width = texture.Width;
            int height = texture.Height;

            Color[] pixelData = new Color[width * height];
            texture.GetData(pixelData); // Get all pixels into 1D array

            Color[] rowAverages = new Color[height];

            for (int y = 0; y < height; y++)
            {
                int r = 0, g = 0, b = 0, a = 0;

                for (int x = 0; x < width; x++)
                {
                    Color c = pixelData[y * width + x];
                    r += c.R;
                    g += c.G;
                    b += c.B;
                    a += c.A;
                }

                int count = width;
                rowAverages[y] = new Color(r / count, g / count, b / count, a / count);
            }

            return rowAverages;
        }

    }

    public class EvolutionEffect
    {
        public DigimonBase Digimon;
        public DigimonBase EvolutionInstance;
        public Color[] RowColorsBase;
        public Color[] RowColorsEvolved;
        public int CurrentRowBase = 0;
        public int CurrentRowEvolved = 0;
        public int Timer = 0;
    }
}
