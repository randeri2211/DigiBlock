using Terraria;
using System;
using DigiBlock.Common;

namespace DigiBlock.Content.Digimon
{
    public abstract class JumpingDigimonBase : DigimonBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AI()
        {
            //Targeting and contact damage
            base.AI();

            if (wildTarget != null && wildTarget.active)
            {
                float moveSpeed = agility * DigiblockConstants.AgilityMoveSpeedMultiplier + 5; // walking speed
                float distanceX = wildTarget.Center.X - NPC.Center.X;

                // Jump towards the target if grounded
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -(float)Math.Sin(45) * moveSpeed;
                }
                NPC.velocity.X = Math.Sign(distanceX) * (float)Math.Cos(45) * moveSpeed;
            }
        }
    }
}