

using System;

namespace DigiBlock.Content.Digimon.Ability
{
    public abstract class DigiAbility
    {
        public DigimonBase digimon;
        public int ManaCost;
        public int coolDown;
        public int currentCoolDown;
        public bool specialScale = true;// true->scales with special attack,false->scales with attack
        public float percentScale = 1f;
        public string name = "";
        public string tooltip = "";
        public DigiAbility(DigimonBase digimon)
        {
            this.digimon = digimon;
        }

        public void _Use()
        {
            if (currentCoolDown == 0)
            {
                int damage;
                if (specialScale)
                {
                    damage = digimon.specialDamage;
                }
                else
                {
                    damage = digimon.physicalDamage;
                }
                Use(digimon.CalculateDamage((int)(damage * percentScale)));
                currentCoolDown = coolDown;
            }
        }

        public void _Update()
        {
            if (currentCoolDown > 0)
            {
                currentCoolDown--;
            }
            Update();
        }

        public virtual void Use(int damage) { }
        public virtual void Update() { }
    }
}