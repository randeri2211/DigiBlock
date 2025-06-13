using Terraria;
using Terraria.ModLoader;

namespace DigiBlock.Content.Damage
{
    public class DigitalDamage : DamageClass
    {
        public override bool UseStandardCritCalcs => true;
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Generic)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false;
        }

        public override void SetDefaultStats(Player player)
        {
            player.GetDamage<DigitalDamage>() -= 1;
            // player.GetCritChance<DigitalDamage>() += 4;
            // player.GetDamage<DigitalDamage>() *= 2; // increases the multiplicative modifier by 2
            // player.GetDamage<DigitalDamage>() += 10; // increases the additive multiplier by 2
        }
    }
}