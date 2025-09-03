using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public abstract class TeleportState : State, IUpdatableState
    {
        protected Transform SelfTransform;
        protected ReactiveVariable<float> TeleportMaxRadius;

        private ReactiveVariable<float> _teleportEnergyCost;
        private ReactiveEvent<Vector3> _teleportedEvent;
        private ICompositeCondition _canTeleport;
        private ReactiveEvent<float> _subtractEnergyCountRequest;
        private ReactiveVariable<float> _teleportedDelay;

        private float _timeDelay = 0;

        public TeleportState(Entity entity)
        {
            SelfTransform = entity.Transform;
            TeleportMaxRadius = entity.TeleportMaxRadius;

            _teleportEnergyCost = entity.TeleportEnergyCost;
            _teleportedDelay = entity.TeleportDelay;
            _teleportedEvent = entity.TeleportedEvent;
            _canTeleport = entity.CanTeleport;
            _subtractEnergyCountRequest = entity.SubtractEnergyCountRequest;
        }

        public void Update(float deltaTime)
        {
            _timeDelay += deltaTime;

            if (_timeDelay > _teleportedDelay.Value && _canTeleport.Evaluate())
            {
                _timeDelay = 0;
                MakeTeleport();
            }
        }

        protected abstract Vector3 GetPosition();

        private void MakeTeleport()
        {
            SelfTransform.position = GetPosition();
            _subtractEnergyCountRequest?.Invoke(_teleportEnergyCost.Value);
            _teleportedEvent?.Invoke(SelfTransform.position);
        }
    }
}