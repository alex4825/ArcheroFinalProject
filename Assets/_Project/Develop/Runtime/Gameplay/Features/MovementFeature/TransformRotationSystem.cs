using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class TransformRotationSystem : IInitializableSystem, IUpdatableSystem
    {
        private Transform _transform;

        private ReactiveVariable<float> _rotationSpeed;
        private ReactiveVariable<Vector3> _direction;

        private ICompositeCondition _canRotate;

        public TransformRotationSystem(Transform transform)
        {
            _transform = transform;
        }

        public void OnInit(Entity entity)
        {
            _rotationSpeed = entity.RotationSpeed;
            _direction = entity.RotationDirection;
            _canRotate = entity.CanRotate;

            if (_direction.Value == Vector3.zero)
                _transform.rotation = Quaternion.LookRotation(_direction.Value.normalized);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canRotate.Evaluate() == false)
                return;

            if (_direction.Value == Vector3.zero)
                return;

            Quaternion lookRotation = Quaternion.LookRotation(_direction.Value.normalized);

            float step = _rotationSpeed.Value / 100 * deltaTime;

            Quaternion rotation = Quaternion.Slerp(_transform.rotation, lookRotation, step);

            _transform.rotation = rotation;
        }
    }
}
