using Terraria.ID;
using Terraria;
using DigiBlock.Content.Digimon.Ability;
using Terraria.ModLoader;
using DigiBlock.Content.Projectiles;

namespace DigiBlock.Content.Digimon
{
    public class Koromon : JumpingDigimonBase
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
            evoStage = Evolutions.InTraining;
            attribute = Attributes.None;
            BubbleBlow ability2 = new BubbleBlow(this);
            specialAbilities.Add(ability2);
            basePhysicalDamage = 10;
            baseSpecialDamage = 5;
            baseAgility = 10;
            NPC.width = 31;
            NPC.height = 20;
            baseMaxHP = 25;
            AnimationType = NPCID.Guide;
            base.SetDefaults();
        }
    }
}