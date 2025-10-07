using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelection
{
    public class NearestToAimInRadiusTargetSelector : ITargetSelector
    {
        private Entity _source;
        private Entity _aim;
        private ReactiveVariable<float> _radius;

        public NearestToAimInRadiusTargetSelector(Entity source, Entity aim, ReactiveVariable<float> radius)
        {
            _source = source;
            _aim = aim;
            _radius = radius;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            IEnumerable<Entity> probableTargetsInRadius = targets.Where(target =>
            {
                bool result = target.HasComponent<TakeDamageRequest>();

                if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage))
                {
                    result = result && canApplyDamage.Evaluate();
                }

                if (_source.TryGetTeam(out ReactiveVariable<Teams> sourceTeam)
                && target.TryGetTeam(out ReactiveVariable<Teams> targetTeam))
                {
                    result = result && sourceTeam.Value != targetTeam.Value;
                }

                result = result && GetDistanceBetween(_source, target) <= _radius.Value;

                result = result && (target != _source);

                return result;
            });

            if (probableTargetsInRadius.Any() == false)
                return null;

            Entity nearestToAimTargetInRadius = probableTargetsInRadius.First();
            float minDistance = GetDistanceBetween(nearestToAimTargetInRadius, _aim);

            foreach (Entity target in probableTargetsInRadius)
            {
                float minDistanceToAim = GetDistanceBetween(nearestToAimTargetInRadius, _aim);

                if (minDistanceToAim < minDistance)
                {
                    minDistanceToAim = minDistance;
                    nearestToAimTargetInRadius = target;
                }
            }

            if (nearestToAimTargetInRadius != null)
            {

            }

            return nearestToAimTargetInRadius;
        }

        private float GetDistanceBetween(Entity entity1, Entity entity2) => (entity1.Transform.position - entity2.Transform.position).magnitude;
    }
}