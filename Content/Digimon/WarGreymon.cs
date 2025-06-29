using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Digimon.Ability;
using DigiBlock.Content.Projectiles;

namespace DigiBlock.Content.Digimon
{
    public class WarGreymon : WalkingDigimonBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Main.npcFrameCount[Type] = 1;
            NPCID.Sets.ExtraFramesCount[Type] = 0; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // The amount of frames in the attacking animation.
        }

        public override void SetDefaults()
        {
            evoStage = Evolutions.Mega;
            lootType = ModContent.GetInstance<Koromon>().Type;
            attribute = Attributes.Vaccine;
            Dash dash = new Dash(this);
            dash.name = "Body Blow";
            specialAbilities.Add(dash);
            TerraForce ability = new TerraForce(this);
            specialAbilities.Add(ability);
            if (basePhysicalDamage == 0)
            {
                baseSpecialDamage = 5;
                basePhysicalDamage = 70;
                baseAgility = 25;
                baseMaxHP = 200;
            }
            NPC.width = 112;
            NPC.height = 104;
            base.SetDefaults();
        }
    }
}