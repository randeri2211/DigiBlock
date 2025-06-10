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
            NPC.width = 32;
            NPC.height = 32;
            baseDmg = 20;
            agility = 25;
            NPC.lifeMax = 15;
            base.SetDefaults();
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient) // Only drop on server
            {
                int cardIndex = Item.NewItem(NPC.GetSource_Loot(), NPC.Hitbox, ModContent.ItemType<DigimonCard>(), 1);

                if (Main.item[cardIndex].ModItem is DigimonCard card)
                {
                    card.setDigimonNpcType(ModContent.GetInstance<Koromon>().Type); // this Digimon's NPC type
                }
                // Try to assign card data post-drop if needed (see Note below)
            }
        }
    }
}