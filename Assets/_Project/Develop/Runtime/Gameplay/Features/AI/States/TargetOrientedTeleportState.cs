using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelection;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class TargetOrientedTeleportState : TeleportState
    {
        private ReactiveVariable<Entity> _currentTarget;

        public TargetOrientedTeleportState(Entity entity, ITargetSelector targetSelector, EntitiesLifeContext entitiesLifeContext) : base(entity)
        {
            _currentTarget = entity.CurrentTarget;
        }

        protected override Vector3 GetPosition()
        {
            Vector3 teleportDirection = _currentTarget.Value.Transform.position - SelfTransform.position;

            float teleportDistance = Mathf.Min(teleportDirection.magnitude, TeleportMaxRadius.Value);

            return SelfTransform.position + teleportDirection.normalized * teleportDistance;
        }
    }
}