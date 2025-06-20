using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon.Ability;

namespace DigiBlock.Content.Digimon
{
    public class Greymon : WalkingDigimonBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = 4;
            NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // The amount of frames in the attacking animation.

        }

        public override void SetDefaults()
        {
            evoStage = Evolutions.Champion;
            lootType = ModContent.GetInstance<Koromon>().Type;
            lootProbablity = 5;
            attribute = Attributes.Vaccine;
            Dash ability1 = new Dash(this);
            ability1.name = "Horn Impulse";
            specialAbilities.Add(ability1);
            FireBall ability2 = new FireBall(this);
            ability2.name = "Bit Fire";
            specialAbilities.Add(ability2);
            if (basePhysicalDamage == 0)
            {
                baseSpecialDamage = 5;
                basePhysicalDamage = 40;
                baseAgility = 25;
                baseMaxHP = 100;
            }
            NPC.width = 67;
            NPC.height = 60;
            base.SetDefaults();
        }
        
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && Main.dayTime && spawnInfo.Player.ZoneForest)
            {
                return 0.1f; // 10% spawn chance relative to other spawns
            }

            return 0f;
        }
    }
}