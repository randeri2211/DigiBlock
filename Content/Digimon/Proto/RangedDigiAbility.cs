using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Projectiles;

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
                    projectileType, // Replace with your custom projectile if needed
                    damage,
                    1f,
                    Main.myPlayer
                );

                if (Main.projectile.IndexInRange(projID))
                {
                    DigiFireballProjectile proj = Main.projectile[projID].ModProjectile as DigiFireballProjectile;
                    if (proj != null)
                    {
                        proj.digimon = digimon;
                    }
                }
            }
        }

        public override void Update()
        {
            
        }
    }
}