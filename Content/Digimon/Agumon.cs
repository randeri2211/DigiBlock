using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon.Ability;

namespace DigiBlock.Content.Digimon
{
    public class Agumon : WalkingDigimonBase
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
            evoStage = Evolutions.Rookie;
            FireBall ability = new FireBall(this);
            ability.name = "Baby Flame";
            specialAbilities.Add(ability);
            lootType = ModContent.GetInstance<Koromon>().Type;
            lootProbablity = 10;
            attribute = Attributes.Vaccine;
            basePhysicalDamage = 20;
            baseSpecialDamage = 20;
            baseAgility = 10;
            NPC.width = 32;
            NPC.height = 32;
            baseMaxHP = 50;
            base.SetDefaults();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneOverworldHeight && Main.dayTime && spawnInfo.Player.ZoneForest)
            {
                return 0.5f; // 10% spawn chance relative to other spawns
            }

            return 0f;
        }
    }
}