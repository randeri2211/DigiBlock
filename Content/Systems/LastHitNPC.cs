using System;
using DigiBlock.Common;
using DigiBlock.Content.Digimon;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Common;

namespace DigiBlock.Content.Systems
{
    public class LastHitNPC : GlobalNPC
    {
        public DigimonBase lastHitByDigimon;

        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
            if (npc.TryGetGlobalNPC(out LastHitNPC victim) && victim.lastHitByDigimon != null)
            {
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
                victim.lastHitByDigimon.GiveEXP(expAmount);
            }
        }
    }
}
