using DigiBlock.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DigiBlock.Content.Digimon.Ability
{
    public class BubbleBlow : RangedDigiAbility
    {

        public BubbleBlow(DigimonBase digimon) : base(digimon)
        {
            name = "Bubble Blow";
            coolDown = 180;
            tooltip = "Blows a Bubble\nDamage scales 100% Special Attack";
            projectileType = ModContent.ProjectileType<BubbleProjectile>();
        }
    }
}