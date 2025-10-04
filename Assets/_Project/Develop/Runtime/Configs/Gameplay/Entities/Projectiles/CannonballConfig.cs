using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/CannoballConfig", fileName = "CannoballConfig")]
    public class CannonballConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Projectiles/Cannoball";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 10f;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 50;
    }
}