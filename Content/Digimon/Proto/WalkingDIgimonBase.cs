using Terraria;
using System;
using DigiBlock.Common;
using Terraria.ID;

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
            AnimationType = NPCID.Zombie;
            base.SetDefaults();
        }

        public override void Move()
        {
            // float moveSpeed = agility * DigiblockConstants.AgilityMoveSpeedMultiplier; // walking speed
            float moveSpeed = 2f + agility * DigiblockConstants.AgilityMoveSpeedMultiplier;
            if (wildTarget != null && wildTarget.active)
            {

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

                if (wildTarget.Center.Y < NPC.Center.Y - 20 && NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -5f; // jump up
                }
            }
            else
            {
                // Tamed without target
                if (NPC.friendly)
                {
                    float distanceX = playerLocation.X - NPC.Center.X;

                    // Apply velocity towards target
                    if (Math.Abs(distanceX) > 10f) // prevent jitter
                    {
                        NPC.velocity.X = Math.Sign(distanceX) * moveSpeed;
                    }
                    else
                    {
                        NPC.velocity.X *= 0.9f; // slow down near target
                    }

                    if (playerLocation.Y < NPC.Center.Y - 20 && NPC.velocity.Y == 0)
                    {
                        NPC.velocity.Y = -5f; // jump up
                    }
                }
                NPC.velocity.X *= 0.9f; // idle drift
            }

        }
    }
}