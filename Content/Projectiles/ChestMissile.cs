using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Systems; // For DigiBlockNPC
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Damage;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace DigiBlock.Content.Projectiles
{
    public class ChestMissile : ProjectileBase
    {

        public override void SetDefaults()
        {
            Projectile.width = 23;
            Projectile.height = 10;
            Projectile.penetrate = 1;
            Projectile.aiStyle = 0;
            base.SetDefaults();
        }

        public override void AI()
        {
            Projectile.spriteDirection = Projectile.velocity.X > 0 ? -1 : 1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.FlipHorizontally;
            effects |= Projectile.spriteDirection == 1 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / Main.projFrames[Projectile.type] / 2);
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRectangle = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.Draw(
                texture,
                drawPos,
                sourceRectangle,
                lightColor,
                Projectile.velocity.ToRotation(),
                drawOrigin,
                Projectile.scale,
                effects,
                0f
            );

            return false; // don't draw default
        }
    }
}
