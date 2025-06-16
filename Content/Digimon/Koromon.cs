using Terraria.ID;
using Terraria;


namespace DigiBlock.Content.Digimon
{
    public class Koromon : JumpingDigimonBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // The amount of frames in the attacking animation.
        }

        public override void SetDefaults()
        {
            evoStage = Evolutions.InTraining;
            attribute = Attributes.None;
            basePhysicalDamage = 5;
            baseAgility = 5;
            NPC.width = 23;
            NPC.height = 12;
            baseMaxHP = 10;
            AnimationType = NPCID.Guide;
            base.SetDefaults();
        }
    }
}