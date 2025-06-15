using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using DigiBlock.Common;
using DigiBlock.Content.Items.Digimon;
using DigiBlock.Content.Damage;
using DigiBlock.Content.Systems;
using System.Collections.Generic;
using DigiBlock.Content.Digimon.Ability;


namespace DigiBlock.Content.Digimon
{
    public enum Evolutions
    {
        InTraining,
        Rookie,
        Champion,
        Ultimate,
        Mega,
    }
    public enum Attributes
    {
        None,
        Vaccine,
        Data,
        Virus,
    }
    public abstract class DigimonBase : ModNPC
    {
        public int lootType;
        public string name;
        public Attributes attribute;
        public DigimonCard card;
        public Player playerOwner;
        public Vector2 playerLocation;
        public float playerDistance;
        public bool justEvolved = false;
        public bool evolving = false;
        public bool summoned = false;
        public bool canMove = true;
        public bool useContactDamage = true;

        public Entity wildTarget = null;
        public bool immune = false;
        
        public List<DigiAbility> specialAbilities = new List<DigiAbility>();
        public int specialAbilityIndex;

        // Stats
        public int contactDamage;
        public int specialDamage;
        public int agility;
        public int defense;
        public int maxHP;
        private int currentEXP = 0;
        public int maxEXP = DigiblockConstants.StartingEXP;
        public int level = 16;
        public Evolutions evoStage;
        public Dictionary<DigimonSpawnBiome, int> biomeKills = new Dictionary<DigimonSpawnBiome,int>();

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
            NPC.damage = contactDamage;
            NPC.lifeMax = maxHP;
            NPC.defense = defense;
            NPC.friendly = false; // Wild digimon by default
        }

        public void copyData(DigimonBase digimon)
        {
            card = digimon.card;
            NPC.friendly = digimon.NPC.friendly;
            level = digimon.level;
            NPC.lifeMax = digimon.NPC.lifeMax;
            NPC.life = digimon.NPC.life;
            NPC.damage = digimon.NPC.damage;
            contactDamage = digimon.contactDamage;
            specialDamage = digimon.specialDamage;
            NPC.defense = digimon.NPC.defense;
            defense = digimon.defense;
            currentEXP = digimon.currentEXP;
            maxEXP = digimon.maxEXP;
            maxHP = digimon.maxHP;
            NPC.active = digimon.NPC.active;
            biomeKills = digimon.biomeKills;
            playerOwner = digimon.playerOwner;
        }

        public override void OnKill()
        {
            if (lootType > 0 && !NPC.friendly)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient) // Only drop on server
                {
                    int cardIndex = Item.NewItem(NPC.GetSource_Loot(), NPC.Hitbox, ModContent.ItemType<DigimonCard>(), 1);

                    if (Main.item[cardIndex].ModItem is DigimonCard card)
                    {
                        card.setDigimonNpcType(lootType);
                    }
                }
            }
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            for (int i = 0; i < specialAbilities.Count; i++)
            {
                specialAbilities[i]?._Update();
            }
            
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
                if (NPC.friendly)
                {
                    if (useContactDamage)
                    {
                        NPC.damage = contactDamage;
                    }
                    
                    ContactDamage();
                }
                if (specialAbilities.Count > specialAbilityIndex)
                {
                    specialAbilities[specialAbilityIndex]?._Use();
                }
            }
            else if (NPC.friendly) // No target and am friendly-> return to player position
            {
                if (playerLocation.X > NPC.Center.X)
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
                NPC.EncourageDespawn(10);
                return;
            }

            // Reset targeting if target is dead
            if (wildTarget != null && !wildTarget.active)
            {
                wildTarget = null;
            }

            if (canMove)
            {
                Move();
            }
        }

        public virtual void Move(){ }

        public override void PostAI()
        {
            if (evolving)
            {
                NPC.velocity = new Vector2();
            }
            base.PostAI();
        }

        public void CalculateStats()
        {
            if (playerOwner != null)
            {
                NPC.lifeMax = (int)(maxHP * playerOwner.GetModPlayer<DigiBlockPlayer>().digimonMaxHPPercent);

                NPC.defense = (int)(defense * playerOwner.GetModPlayer<DigiBlockPlayer>().digimonDefensePercent);
            }
            else
            {
                NPC.lifeMax = maxHP;
                NPC.defense = defense;
            }

        }

        public int CalculateDamage(int damage, Attributes targetAttribute = Attributes.None)
        {
            // Increase based on attributes
            if ((attribute == Attributes.Data && targetAttribute == Attributes.Vaccine) || (attribute == Attributes.Virus && targetAttribute == Attributes.Data) || (attribute == Attributes.Vaccine && targetAttribute == Attributes.Virus))
            {
                damage = damage + (int)(damage * DigiblockConstants.AttributeDamageMultiplier);
            }
            else if (attribute != targetAttribute && attribute != Attributes.None && targetAttribute != Attributes.None)
            {
                damage = (int)(damage * DigiblockConstants.AttributeDamageMultiplier);
            }
            if (playerOwner != null)
            {
                var damageModifier = playerOwner.GetDamage<DigitalDamage>();
                damage = (int)((damage + damageModifier.Additive) * damageModifier.Multiplicative);
            }
            return damage;
        }

        private void TargetHostileNPC()
        {
            int closestEnemy = -1;
            float closestDistance = NPCID.Sets.DangerDetectRange[Type]; // Use the defined range
            float maxDistance = NPCID.Sets.DangerDetectRange[Type];
            if (playerOwner == null)
            {
                playerOwner = card.digivice.FindPlayer();
                CalculateStats();
            }

            playerLocation = playerOwner.Center;
            playerDistance = Vector2.Distance(playerLocation, NPC.Center);
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (target.active && !target.friendly && !target.townNPC && target.CanBeChasedBy(this))
                {
                    float distance = Vector2.Distance(NPC.Center, target.Center);
                    float targetStartDistance = Vector2.Distance(playerLocation, target.Center);
                    if (distance < closestDistance && targetStartDistance < maxDistance)
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
                if (playerDistance > maxDistance)
                {
                    NPC.Center = playerLocation;
                }
                NPC.target = 255;
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

        public int getEXP()
        {
            return currentEXP;
        }

        public void GiveEXP(int exp)
        {
            if (exp > 0)
            {
                currentEXP += exp;
            }
            CheckLevelUp();
        }

        public void CheckLevelUp()
        {
            if (currentEXP >= maxEXP)
            {
                currentEXP -= maxEXP;
                level += 1;
                maxEXP = (int)(maxEXP * DigiblockConstants.LevelingEXPMultiplier);
                agility += DigiblockConstants.LevelUpBonus;
                maxHP += DigiblockConstants.LevelUpBonus * DigiblockConstants.HPLevelUpBonusMultiplier;
                contactDamage += DigiblockConstants.LevelUpBonus;
                specialDamage += DigiblockConstants.LevelUpBonus;
                NPC.defense += DigiblockConstants.LevelUpBonus;
                CheckLevelUp();
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (immune)
            {
                return false;
            }
            return !NPC.friendly;
        }

        public override bool CanHitNPC(NPC target)
        {
            if (immune)
            {
                return false;
            }
            return !NPC.friendly;
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
            if (immune)
            {
                return false;
            }
            return NPC.friendly != attacker.friendly;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (immune)
            {
                return false;
            }
            return !NPC.friendly;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (immune)
            {
                return false;
            }
            return !NPC.friendly;
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
                            Attributes targetAttribute = Attributes.None;
                            if (target.ModNPC is DigimonBase targetDigimon)
                            {
                                targetAttribute = targetDigimon.attribute;
                            }
                            // Apply contact damage manually
                            NPC.HitInfo hitInfo = new()
                            {
                                // Use the damage modifiers of the owner player in damage calculations
                                Damage = CalculateDamage(NPC.damage, targetAttribute),
                                Knockback = 1f,
                                HitDirection = NPC.direction,
                                DamageType = ModContent.GetInstance<DigitalDamage>(),
                            };


                            // Store this Digimon in the target's GlobalNPC
                            if (target.TryGetGlobalNPC(out DigiBlockNPC victim))
                            {
                                victim.lastHitByDigimon = this;
                            }
                            target.StrikeNPC(hitInfo);

                            // Add immunity time to prevent rapid hits
                            target.immune[NPC.whoAmI] = 30; // 30 ticks = 0.5s
                        }
                    }
                }
            }
        }

        public virtual void WildAI() { }
        public virtual void TamedAI() { }

    }
}