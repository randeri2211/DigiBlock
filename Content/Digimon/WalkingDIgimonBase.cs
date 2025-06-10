using Terraria;
using System;
using DigiBlock.Common;

namespace DigiBlock.Content.Digimon
{
    public abstract class WalkingDigimonBase : DigimonBase
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
                float moveSpeed = agility * DigiblockConstants.AgilityMoveSpeedMultiplier; // walking speed
                float distanceX = wildTarget.Center.X - NPC.Center.X;

                // Apply velocity towards target
                if (Math.Abs(distanceX) > 10f) // prevent jitter
                {
                    NPC.velocity.X = Math.Sign(distanceX) * moveSpeed;
                }
                else
                {
                    NPC.velocity.X *= 0.9f; // slow down near target
                }

                // Optional: jump if target is above and we're grounded
                if (wildTarget.Center.Y < NPC.Center.Y - 20 && NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -5f; // jump up
                }
            }
            else
            {
                NPC.velocity.X *= 0.9f; // idle drift
            }

        }
    }
}