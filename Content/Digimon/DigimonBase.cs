using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DigiBlock.Content.Digimon
{
    public enum Evolutions
    {
        InTraining,
        Rookie,
        Champion,
        Ultimate,
        Mega
    }
    public class DigimonBase : ModNPC
    {
        public bool tamed = true;
        public Entity wildTarget = null;
        public int level = 16;
        public Evolutions evoStage;
        public bool justEvolved = false;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
            NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.friendly = tamed; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            // NPC.damage = 10;
            // NPC.defense = 15;
            NPC.lifeMax = 250;
            // NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            // Targeting
            if (tamed)
            {
                TargetHostileNPC();
                if (NPC.HasValidTarget)
                {
                    // Console.WriteLine("Targeting: " + Main.npc[NPC.target].FullName);

                    TamedAI();
                }


            }
            else
            {
                if (wildTarget == null)
                {
                    TargetFriendlyDigimonPlayer();
                    // Console.WriteLine("Targeting: " + wildTarget.ToString() + " in target");
                }


                Player player = Main.player[NPC.target];

                if (!wildTarget.active)
                {
                    NPC.EncourageDespawn(10);
                    return;
                }

                WildAI();

            }

        }

        private void TargetHostileNPC()
        {
            int closestEnemy = -1;
            float closestDistance = NPCID.Sets.DangerDetectRange[Type]; // Use the defined range

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (target.active && !target.friendly && !target.townNPC && target.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(NPC.Center, target.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = i;
                    }
                }
            }

            if (closestEnemy != -1)
            {
                NPC.target = closestEnemy; // Ensure the NPC isn't auto-targeting players
                NPC.direction = (Main.npc[closestEnemy].Center.X > NPC.Center.X) ? 1 : -1;
            }
            else
            {
                NPC.target = 255;
                NPC.velocity *= 0.95f; // idle
            }
        }

        private void TargetFriendlyDigimonPlayer()
        {
            float closestDistance = NPCID.Sets.DangerDetectRange[Type]; // Use the defined range

            // Check all players
            for (int i = 0; i < Main.player.Length; i++)
            {
                Player player = Main.player[i];

                if (player.active && !player.dead)
                {
                    float distance = Vector2.Distance(NPC.Center, player.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        wildTarget = player;
                    }
                }
            }

            // Check all NPCs (to find tamed Digimon)
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC potential = Main.npc[i];

                if (potential.active && potential.ModNPC is DigimonBase digimon && digimon.tamed)
                {
                    float distance = Vector2.Distance(NPC.Center, potential.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        wildTarget = potential;
                    }
                }
            }

        }

        /// <summary>
        /// Can hit the player only if its a wild digimon
        /// </summary>
        /// <param name="target"></param>
        /// <param name="cooldownSlot"></param>
        /// <returns></returns>
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !tamed;
        }

        /// <summary>
        /// Can hit other npcs only if they are not friendly
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public override bool CanHitNPC(NPC target)
        {
            if (tamed)
            {
                return !target.friendly;
            }
            else
            {
                return target.friendly;
            }
        }

        /// <summary>
        /// Used to decide how to move the digimon/how to attack the target when the digimon is tamed
        /// </summary>
        public void TamedAI()
        {

        }

        /// <summary>
        /// Used to decide how to move the digimon/how to attack the target when the digimon is wild 
        /// </summary>
        public void WildAI()
        {

        }

    }
}