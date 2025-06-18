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

        public override void Move()
        {
            if (NPC.velocity.Y == 0)
            {
                NPC.velocity.X = 0;
            }
            float moveSpeed = agility * DigiblockConstants.AgilityMoveSpeedMultiplier + 5; // jump speed
            if (wildTarget != null && wildTarget.active)
            {
                float distanceX = wildTarget.Center.X - NPC.Center.X;

                // Jump towards the target if grounded
                if (NPC.velocity.Y == 0)
                {
                    NPC.velocity.Y = -(float)Math.Sin(45) * moveSpeed;
                    NPC.velocity.X = Math.Sign(distanceX) * (float)Math.Cos(45) * moveSpeed;
                }
            }
            else
            {
                if (NPC.friendly)
                {
                    // Jump towards the player
                    if (playerOwner != null && playerDistance > 30f)
                    {
                        Console.WriteLine("playerdistance"+playerDistance);
                        float xDiff = playerOwner.Center.X - NPC.Center.X;
                        // Jump towards the target if grounded
                        if (NPC.velocity.Y == 0)
                        {
                            NPC.velocity.Y = -(float)Math.Sin(45) * moveSpeed;
                            NPC.velocity.X = Math.Sign(xDiff) * (float)Math.Cos(45) * moveSpeed;
                        }
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.Y == 0)
            {
                // On ground
                if (Math.Abs(NPC.velocity.X) > 0.1f)
                {
                    // Walking or about to jump
                    NPC.frame.Y = frameHeight * 1; // Frame 1: preparing to jump
                }
                else
                {
                    NPC.frame.Y = 0; // Frame 0: idle
                }
            }
            else if (NPC.velocity.Y < 0)
            {
                // Ascending
                NPC.frame.Y = frameHeight * 2; // Frame 2
            }
            else
            {
                // Descending
                NPC.frame.Y = frameHeight * 3; // Frame 3
            }
        }
    }
}