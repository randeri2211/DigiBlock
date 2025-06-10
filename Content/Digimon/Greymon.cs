using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Items.Digimon;


namespace DigiBlock.Content.Digimon
{
    public class Greymon : WalkingDigimonBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = 3;
            NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // The amount of frames in the attacking animation.
            evoStage = Evolutions.Champion;
        }

        public override void SetDefaults()
        {
            lootType = ModContent.GetInstance<Koromon>().Type;
            NPC.width = 32;
            NPC.height = 32;
            baseDmg = 20;
            agility = 25;
            NPC.lifeMax = 15;
            base.SetDefaults();
        }
    }
}