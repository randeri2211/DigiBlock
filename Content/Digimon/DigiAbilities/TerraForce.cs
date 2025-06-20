using DigiBlock.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DigiBlock.Content.Digimon.Ability
{
    public class TerraForce : RangedDigiAbility
    {
        private int timer = 0;
        public TerraForce(DigimonBase digimon) : base(digimon)
        {
            name = "Terra Force";
            coolDown = 300;
            tooltip = "Takes all of the energy within the atmosphere and concentrates it into one spot\nthen fires it as an extremely dense, high-temperature energy shot.\nDamage scales 200% Special Attack";
            projectileType = ModContent.ProjectileType<TerraForceProjectile>();
        }

        public override void Use(int damage)
        {
            digimon.canMove = false;
            digimon.NPC.velocity = new Vector2(0, 0);
            timer = ModContent.GetInstance<TerraForceProjectile>().animationTime;
            base.Use(damage);
        }

        public override void Update()
        {
            if (timer > 0)
            {
                timer--;
            }
            else
            {
                digimon.canMove = true;
            }
            base.Update();
        }
    }
}