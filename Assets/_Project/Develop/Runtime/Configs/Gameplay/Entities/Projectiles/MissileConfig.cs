using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/MissileConfig", fileName = "MissileConfig")]
    public class MissileConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Projectiles/Missile";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 20;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 3;
    }
}