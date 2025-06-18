using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Systems; // For DigiBlockNPC
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Damage;
using Microsoft.Xna.Framework;
using System;

namespace DigiBlock.Content.Projectiles
{
    public class BubbleProjectile : ProjectileBase
    {
        float accelY;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            accelY = 0;
            float velSize = Projectile.velocity.Length();
            Projectile.velocity.X = velSize;
            Projectile.velocity.Y = 0;
            base.SetDefaults();
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            Projectile.velocity.X *= 0.98f;
            Projectile.velocity.Y += accelY;
            Projectile.velocity.Y = Math.Max(Projectile.velocity.Y, -5f);
            accelY -= 0.001f;
        }
    }
}
