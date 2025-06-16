using DigiBlock.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DigiBlock.Content.Digimon.Ability
{
    public class FireBall : RangedDigiAbility
    {

        public FireBall(DigimonBase digimon) : base(digimon)
        {
            coolDown = 60;
            tooltip = "Shoots Fireballs\nDamage scales 100% Special Attack";
            projectileType = ModContent.ProjectileType<FireballProjectile>();
        }
    }
}