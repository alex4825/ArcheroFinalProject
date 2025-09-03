using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelection
{
    public class MinHPTargetSelector : ITargetSelector
    {
        private Entity _source;

        public MinHPTargetSelector(Entity entity)
        {
            _source = entity;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            IEnumerable<Entity> selectedTargets = targets.Where(target =>
            {
                bool result = target.HasComponent<TakeDamageRequest>();

                if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage))
                {
                    result = result && canApplyDamage.Evaluate();
                }

                result = result && target != _source;

                return result;
            });

            if (selectedTargets.Any() == false)
                return null;

            float minHealth = selectedTargets.Min(targets => targets.CurrentHealth.Value);
            Entity mostDamagedTarget = selectedTargets.First(target => target.CurrentHealth.Value == minHealth);

            return mostDamagedTarget;
        }
    }
}