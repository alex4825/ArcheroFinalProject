using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class CharacterControllerRotationSystem : RotationSystem
    {
        private CharacterController _characterController;

        public override void OnInit(Entity entity)
        {
            base.OnInit(entity);
            _characterController = entity.CharacterController;
        }

        protected override Quaternion CurrentRotation
        {
            get => _characterController.transform.rotation;
            set => _characterController.transform.rotation = value;
        }
    }
}
