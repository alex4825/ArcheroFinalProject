using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesHelper
    {
        public static bool TryTakeDamageFrom(Entity source, Entity damageable, float damage)
        {
            if (damageable.TryGetTakeDamageRequest(out ReactiveEvent<float> takeDamageRequest) == false)
                return false;

            if (source.TryGetTeam(out ReactiveVariable<Teams> sourceTeam)
                && damageable.TryGetTeam(out ReactiveVariable<Teams> damageableTeam))
            {
                if (sourceTeam.Value == damageableTeam.Value)
                    return false;
            }

            if (damageable.HasComponent<IsKilled>() && damageable.CurrentHealth.Value - damage <= 0)
                damageable.IsKilled.Value = true;

            takeDamageRequest.Invoke(damage);
            return true;
        }

        public static bool TryTakeDamageFrom(Teams team, Entity damageable, float damage)
        {
            if (damageable.TryGetTakeDamageRequest(out ReactiveEvent<float> takeDamageRequest) == false)
                return false;

            if (damageable.TryGetTeam(out ReactiveVariable<Teams> damageableTeam))
                if (team == damageableTeam.Value)
                    return false;

            if (damageable.HasComponent<IsKilled>() && damageable.CurrentHealth.Value - damage <= 0)
                damageable.IsKilled.Value = true;

            takeDamageRequest.Invoke(damage);
            return true;
        }
    }
}