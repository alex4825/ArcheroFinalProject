using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportState : TeleportState
    {
        public RandomTeleportState(Entity entity) : base(entity)
        {
        }

        protected override Vector3 GetPosition()
        {
            float randomAngle = Random.Range(0, 360f);
            float randomDirectionLength = Random.Range(0, TeleportMaxRadius.Value);
            Vector3 moveDirectionNormalized = (Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward).normalized;

            return SelfTransform.position + moveDirectionNormalized * randomDirectionLength;
        }
    }
}