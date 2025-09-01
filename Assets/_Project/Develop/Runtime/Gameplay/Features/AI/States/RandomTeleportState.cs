using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportState : State, IUpdatableState
    {
        private ReactiveVariable<float> _teleportEnergyCost;
        private ReactiveVariable<float> _teleportMaxRadius;
        private ReactiveEvent<Vector3> _teleportedEvent;
        private Transform _selfTransform;
        private ReactiveEvent<float> _subtractEnergyCountRequest;

        public RandomTeleportState(Entity entity)
        {
            _teleportEnergyCost = entity.TeleportEnergyCost;
            _teleportMaxRadius = entity.TeleportMaxRadius;
            _teleportedEvent = entity.TeleportedEvent;

            _selfTransform = entity.Transform;

            _subtractEnergyCountRequest = entity.SubtractEnergyCountRequest;
        }

        public override void Enter()
        {
            base.Enter();

            _selfTransform.position = GetRandomPosition();

            _subtractEnergyCountRequest?.Invoke(_teleportEnergyCost.Value);

            _teleportedEvent?.Invoke(_selfTransform.position);
        }

        public void Update(float deltaTime)
        {
        }

        private Vector3 GetRandomPosition()
        {
            float randomAngle = Random.Range(0, 360f);
            float randomDirectionLength = Random.Range(0, _teleportMaxRadius.Value);
            Vector3 moveDirectionNormalized = (Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward).normalized;

            return moveDirectionNormalized * randomDirectionLength;
        }
    }
}