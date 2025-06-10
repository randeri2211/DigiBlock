using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Common;
using DigiBlock.Content.Items.Digimon;

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
    public abstract class DigimonBase : ModNPC
    {
        public string name;
        public DigimonCard card;
        public bool justEvolved = false;
        public bool summoned = false;
        public Entity wildTarget = null;
        public int level = 16;
        public Evolutions evoStage;
        private int currentEXP = 0;
        private int maxEXP = DigiblockConstants.StartingEXP;
        // Stats
        // HP and maxHP are already implemented as NPC.life and NPC.lifeMax
        public int baseDmg;
        public int agility;

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
            name = Name;
            NPC.damage = baseDmg;

            NPC.friendly = false; // Wild digimon by default
        }

        public void copyData(DigimonBase digimon)
        {
            // tamed = digimon.tamed;
            card = digimon.card;
            NPC.friendly = digimon.NPC.friendly;
            level = digimon.level;
            NPC.lifeMax = digimon.NPC.lifeMax;
            NPC.life = digimon.NPC.life;
            NPC.damage = digimon.NPC.damage;
            baseDmg = digimon.baseDmg;
            NPC.active = digimon.NPC.active;
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            // Targeting
            if (wildTarget == null)
            {
                // Console.WriteLine("Aiming: ");
                if (NPC.friendly)
                {
                    TargetHostileNPC();
                }
                else
                {
                    TargetFriendlyDigimonPlayer();
                }
            }
            if (wildTarget != null)
            {
                // Console.WriteLine("Targeting: " + wildTarget);
                // Turn towards the target
                if (wildTarget.Center.X > NPC.Center.X)
                {
                    NPC.direction = 1;
                }
                else
                {
                    NPC.direction = -1;
                }
                NPC.spriteDirection = NPC.direction;

                ContactDamage();
            }
            if (!NPC.friendly && wildTarget == null)
            {
                NPC.EncourageDespawn(10);
                return;
            }

            // Reset targeting if target is dead
            if (wildTarget != null && !wildTarget.active)
            {
                wildTarget = null;
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
                wildTarget = Main.npc[closestEnemy];
                if (wildTarget.Center.X > NPC.Center.X)
                {
                    NPC.direction = 1;
                }
                else
                {
                    NPC.direction = -1;
                }
                NPC.spriteDirection = NPC.direction;
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

                if (potential.active && potential.ModNPC is DigimonBase digimon && digimon.NPC.friendly)
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

        public void GiveEXP(int exp)
        {
            if (exp > 0)
            {
                currentEXP += exp;
            }
        }

        public void CheckLevelUp()
        {
            if (currentEXP >= maxEXP)
            {
                currentEXP -= maxEXP;
                level += 1;
                maxEXP = (int)(maxEXP * DigiblockConstants.LevelingEXPMultiplier);
                CheckLevelUp();
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.friendly;
        }

        public override bool CanHitNPC(NPC target)
        {
            return !NPC.friendly;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit)
        {
            base.OnHitNPC(target, hit);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            base.OnHitPlayer(target, hurtInfo);
        }

        public void ContactDamage()
        {
            foreach (NPC target in Main.npc)
            {
                if (target.active && NPC.friendly != target.friendly && target.CanBeChasedBy(NPC))
                {
                    // Simple bounding box check for contact
                    if (NPC.Hitbox.Intersects(target.Hitbox))
                    {
                        // Optional: cooldown check
                        if (target.immune[NPC.whoAmI] <= 0)
                        {
                            int damage = baseDmg;

                            // Apply contact damage manually
                            NPC.HitInfo hitInfo = new()
                            {
                                Damage = damage,
                                Knockback = 1f,
                                HitDirection = NPC.direction
                            };

                            target.StrikeNPC(hitInfo);

                            // Add immunity time to prevent rapid hits
                            target.immune[NPC.whoAmI] = 30; // 30 ticks = 0.5s
                        }
                    }
                }
            }
        }

        public virtual void WildAI() { }
        public virtual void TamedAI(){}

    }
}