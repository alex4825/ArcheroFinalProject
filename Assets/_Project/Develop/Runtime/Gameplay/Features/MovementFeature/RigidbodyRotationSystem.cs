using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class RigidbodyRotationSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private const float MinRotationAngle = 0.05f;

        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _rotationSpeed;
        private Rigidbody _rigidbody;

        private Quaternion _targetRotation;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _rotationSpeed = entity.RotationSpeed;
            _rigidbody = entity.Rigidbody;

            _moveDirection.Subscribe(OnMoveDirectionChanged);
        }

        private void OnMoveDirectionChanged(Vector3 oldDirection, Vector3 newDirection)
        {
            if (newDirection == Vector3.zero)
            {
                _targetRotation = _rigidbody.rotation;
                return;
            }

            _targetRotation = Quaternion.LookRotation(newDirection, Vector3.up);
        }

        public void OnUpdate(float deltaTime)
        {
            if (Quaternion.Angle(_rigidbody.rotation, _targetRotation) > MinRotationAngle)
            {
                _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, _targetRotation, deltaTime * _rotationSpeed.Value).normalized;
            }
        }

        public void OnDispose(Entity entity)
        {
            entity.Dispose();
        }
    }
}
