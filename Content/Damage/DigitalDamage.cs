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
        
        public override void SetDefaultStats(Player player) {
            // player.GetCritChance<DigitalDamage>() += 4;
		}
    }
}