using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public abstract class RotationSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private const float MinRotationAngle = 0.05f;

        private ReactiveVariable<Vector3> _moveDirection;
        private ReactiveVariable<float> _rotationSpeed;

        private Quaternion _targetRotation;

        protected abstract Quaternion CurrentRotation { get; set; }

        public virtual void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _rotationSpeed = entity.RotationSpeed;

            _moveDirection.Subscribe(OnMoveDirectionChanged);
        }

        private void OnMoveDirectionChanged(Vector3 oldDirection, Vector3 newDirection)
        {
            if (newDirection == Vector3.zero)
            {
                _targetRotation = CurrentRotation;
                return;
            }

            _targetRotation = Quaternion.LookRotation(newDirection, Vector3.up);
        }

        public void OnUpdate(float deltaTime)
        {
            if (Quaternion.Angle(CurrentRotation, _targetRotation) > MinRotationAngle)
            {
                CurrentRotation = Quaternion.Slerp(CurrentRotation, _targetRotation, deltaTime * _rotationSpeed.Value).normalized;
            }
        }

        public void OnDispose(Entity entity)
        {
            entity.Dispose();
        }
    }
}
