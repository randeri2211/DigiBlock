using DigiBlock.Common;
using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace DigiBlock.Content.Systems
{
    public enum DigimonSpawnBiome
    {
        Unknown,
        Forest,
        Jungle,
        Snow,
        Desert,
        Ocean,
        Evil,
        Underworld,
        Corruption,
        Crimson,
        Hallow,
        Dungeon,
        Sky
    }
    public class DigiBlockNPC : GlobalNPC
    {
        public DigimonBase lastHitByDigimon;
        public DigimonSpawnBiome SpawnBiome = DigimonSpawnBiome.Unknown;

        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
            DigiBlockNPC victim = npc.GetGlobalNPC<DigiBlockNPC>();
            // Killed by tamed digimon
            if (victim.lastHitByDigimon != null)
            {
                // Handle Exp
                int expAmount = 0;
                if (npc.ModNPC is DigimonBase digimon)
                {
                    // Digimon exp scales with level(which will also scale with hardmode)
                    expAmount = (int)(digimon.level * DigiblockConstants.DigimonLevelExpKillMultiplier);
                }
                else
                {
                    // Non digimon exp scales with hardmode
                    expAmount = (Main.hardMode ? 2 : 1) * 10;
                }

                victim.lastHitByDigimon.GiveEXP((int)(expAmount * victim.lastHitByDigimon.playerOwner.GetModPlayer<DigiBlockPlayer>().digimonEXPPercent));

                //Handle biome kill count
                if (victim.SpawnBiome == DigimonSpawnBiome.Unknown)
                {
                    victim.SpawnBiome = DetectBiome(npc.position); // recheck just before death
                }
                if (victim.lastHitByDigimon.biomeKills.TryGetValue(victim.SpawnBiome, out int killCount))
                {
                    victim.lastHitByDigimon.biomeKills[victim.SpawnBiome] = killCount + 1;
                }
                else
                {
                    victim.lastHitByDigimon.biomeKills.Add(victim.SpawnBiome, 1);
                }
            }
        }

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            npc.GetGlobalNPC<DigiBlockNPC>().SpawnBiome = DetectBiome(npc.position);
        }

        private DigimonSpawnBiome DetectBiome(Vector2 position)
        {
            Player closestPlayer = Main.player[Player.FindClosest(position, 1, 1)];

            if (closestPlayer.ZoneUnderworldHeight) return DigimonSpawnBiome.Underworld;
            if (closestPlayer.ZoneSkyHeight) return DigimonSpawnBiome.Sky;
            if (closestPlayer.ZoneDungeon) return DigimonSpawnBiome.Dungeon;
            if (closestPlayer.ZoneSnow) return DigimonSpawnBiome.Snow;
            if (closestPlayer.ZoneJungle) return DigimonSpawnBiome.Jungle;
            if (closestPlayer.ZoneCorrupt) return DigimonSpawnBiome.Evil;
            if (closestPlayer.ZoneCrimson) return DigimonSpawnBiome.Evil;
            if (closestPlayer.ZoneHallow) return DigimonSpawnBiome.Hallow;
            if (closestPlayer.ZoneDesert) return DigimonSpawnBiome.Desert;
            if (closestPlayer.ZoneBeach) return DigimonSpawnBiome.Ocean;
            if (closestPlayer.ZoneOverworldHeight) return DigimonSpawnBiome.Forest;

            return DigimonSpawnBiome.Unknown;
        }

    }
}
