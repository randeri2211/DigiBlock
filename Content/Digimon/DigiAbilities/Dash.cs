using Microsoft.Xna.Framework;
using Terraria;

namespace DigiBlock.Content.Digimon.Ability
{
    public class Dash : DigiAbility
    {
        private bool isDashing = false;
        private Vector2 dashVelocity;
        private int dashDuration = 60; // 20 ticks = ~1/3 of a second
        private int dashTimer = 0;

        public Dash(DigimonBase digimon) : base(digimon)
        {
            name = "Dash";
            coolDown = 300;
            tooltip = "Dash speed scales with agility\nDamage scales 100% Special Attack";
        }

        public override void Use(int damage)
        {
            Vector2 direction = digimon.wildTarget.Bottom - digimon.NPC.Bottom;
            dash(direction, damage);
        }

        public override void Update()
        {
            if (isDashing)
            {
                digimon.NPC.velocity = dashVelocity;
                dashTimer--;
                if (dashTimer <= 0)
                {
                    digimon.NPC.damage = digimon.CalculateDamage(digimon.physicalDamage);
                    digimon.canMove = true;
                    digimon.useContactDamage = true;
                    // digimon.NPC.velocity = Vector2.Zero;
                    digimon.immune = false;
                    isDashing = false;
                }
            }
        }

        public void dash(Vector2 direction, int damage)
        {
            if (digimon.wildTarget != null && digimon.wildTarget.active)
            {
                digimon.NPC.damage = damage;
                digimon.canMove = false;
                digimon.useContactDamage = false;
                direction.Normalize();
                dashVelocity = direction * digimon.agility * 0.3f; // Speed of dash
                isDashing = true;
                dashTimer = dashDuration;
                digimon.immune = true;
            }
        }
    }
}