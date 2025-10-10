using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Heal
{
    public class Healer
    {
        public static void Heal(Entity entity, float healKoef)
        {
            float currentHealth = entity.CurrentHealth.Value;
            float maxHealth = entity.MaxHealth.Value;

            entity.CurrentHealth.Value = MathF.Min(currentHealth + maxHealth * (healKoef - 1), maxHealth);
        }
    }
}