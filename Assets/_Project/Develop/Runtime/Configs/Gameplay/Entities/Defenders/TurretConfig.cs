using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/TurretConfig", fileName = "TurretConfig")]
    public class TurretConfig : DefenderConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/TurretConfig";
        [field: SerializeField] public MissileConfig MissileConfig { get; private set; }
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 120;
        [field: SerializeField, Min(0)] public float Damage { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float AttackProcessTime { get; private set; } = 1f;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 2.5f;
        [field: SerializeField, Min(0)] public float MaxAttackDistance { get; private set; } = 4f;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1;
    }
}