using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Systems; // For DigiBlockNPC
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Damage;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

namespace DigiBlock.Content.Projectiles
{
    public class TerraForceProjectile : ProjectileBase
    {
        public  float maxScale;
        public int animationTime;
        public float step;
        int direction = 0;
        float yOffset;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;
            Projectile.scale = 0f;
            Projectile.alpha = 100;
            maxScale = 30f;
            animationTime = 120;
            step = maxScale / animationTime;            
        }

        public override bool PreAI()
        {
            return true;
        }

        public override void AI()
        {
            if (Projectile.scale < maxScale)
            {
                Texture2D texture2D = TextureAssets.Projectile[Projectile.type].Value;
                // start still
                if (Projectile.velocity.X != 0)
                {
                    yOffset = texture2D.Height * maxScale / 2 + 20f;
                    // Projectile.position.Y -= yOffset;

                    direction = (Projectile.velocity.X > 0) ? 1 : -1;
                    Projectile.velocity.X = 0;
                    Projectile.velocity.Y = 0;
                }
                Projectile.scale += step;
                Projectile.position.Y = digimon.NPC.position.Y - yOffset - texture2D.Height * Projectile.scale / 2;
                Projectile.position.X = digimon.NPC.position.X;

            }
            else
            {
                Projectile.velocity.X = direction * (float)Math.Cos(45) * 5f;
                Projectile.velocity.Y = (float)Math.Sin(45) * 5f;
            }
        }
    }
}
