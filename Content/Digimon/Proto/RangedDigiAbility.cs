using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Projectiles;
using System;

namespace DigiBlock.Content.Digimon.Ability
{
    public abstract class RangedDigiAbility : DigiAbility
    {
        public int projectileType;
        public RangedDigiAbility(DigimonBase digimon) : base(digimon)
        {

        }

        public override void Use(int damage)
        {
            if (digimon.wildTarget != null && digimon.wildTarget.active)
            {
                Vector2 direction = digimon.wildTarget.Center - digimon.NPC.Center;
                direction.Normalize();

                Vector2 velocity = direction * 10f;

                int projID = Projectile.NewProjectile(
                    digimon.NPC.GetSource_FromAI(),
                    digimon.NPC.Center,
                    velocity,
                    projectileType,
                    damage,
                    1f,
                    255
                );

                if (Main.projectile.IndexInRange(projID))
                {
                    ProjectileBase proj = Main.projectile[projID].ModProjectile as ProjectileBase;
                    if (proj != null)
                    {
                        proj.digimon = digimon;
                        proj.Projectile.friendly = digimon.NPC.friendly;
                        proj.Projectile.hostile = !digimon.NPC.friendly;
                        Console.WriteLine("proj friendly " + proj.Projectile.friendly);
                    }
                }
            }
        }

        public override void Update()
        {
            
        }
    }
}