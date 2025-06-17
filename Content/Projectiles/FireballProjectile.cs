using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Systems; // For DigiBlockNPC
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Damage;

namespace DigiBlock.Content.Projectiles
{
    public class FireballProjectile : ProjectileBase
    {

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            base.SetDefaults();
        }
    }
}
