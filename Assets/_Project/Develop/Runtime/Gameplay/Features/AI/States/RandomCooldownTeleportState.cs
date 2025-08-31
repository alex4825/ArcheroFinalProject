using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomCooldownTeleportState : State, IUpdatableState
    {
        private ReactiveVariable<float> _teleportEnergyCost;
        private ReactiveVariable<float> _teleportMaxRadius;
        private ReactiveEvent<Vector3> _teleportedEvent;
        private Transform _selfTransform;
        private ReactiveEvent<float> _subtractEnergyCountRequest;
        private ICompositeCondition _canTeleport;

        private float _cooldownBetweenTeleport;
        private float _timer;

        private IDisposable _enteredDisposable;

        private bool _entered;

        public RandomCooldownTeleportState(Entity entity, float cooldownBetweenTeleport)
        {
            _teleportEnergyCost = entity.TeleportEnergyCost;
            _teleportMaxRadius = entity.TeleportMaxRadius;
            _teleportedEvent = entity.TeleportedEvent;

            _selfTransform = entity.Transform;

            _subtractEnergyCountRequest = entity.SubtractEnergyCountRequest;

            _canTeleport = entity.CanTeleport;

            _cooldownBetweenTeleport = cooldownBetweenTeleport;
        }

        public override void Enter()
        {
            base.Enter();

            _timer = 0;

            _enteredDisposable = Entered.Subscribe(() => _entered = true);
        }

        public override void Exit()
        {
            base.Exit();

            _enteredDisposable.Dispose();

            _entered = false;
        }

        public void Update(float deltaTime)
        {
            if (_entered == false)
                return;

            _timer += deltaTime;

            if (_timer >= _cooldownBetweenTeleport && _canTeleport.Evaluate())
            {
                _timer = 0;

                _selfTransform.position = GetRandomPosition();

                _subtractEnergyCountRequest?.Invoke(_teleportEnergyCost.Value);

                _teleportedEvent?.Invoke(_selfTransform.position);
            }
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