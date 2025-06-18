using Terraria;
using Terraria.ModLoader;
using DigiBlock.Content.Systems; // For DigiBlockNPC
using DigiBlock.Content.Digimon;
using DigiBlock.Content.Damage;
using System;

namespace DigiBlock.Content.Projectiles
{
    public abstract class ProjectileBase : ModProjectile
    {
        public DigimonBase digimon;

        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<DigitalDamage>();
        }

        public override bool? CanHitNPC(NPC target)
        {
            Console.WriteLine("check");
            return target.friendly != digimon.NPC.friendly;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.TryGetGlobalNPC(out DigiBlockNPC globalTarget))
            {
                globalTarget.lastHitByDigimon = digimon;
            }
        }

        public override bool CanHitPlayer(Player target)
        {
            return !digimon.NPC.friendly;
        }

        
    }
}
