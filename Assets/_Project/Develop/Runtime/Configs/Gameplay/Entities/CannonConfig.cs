using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/CannonConfig", fileName = "CannonConfig")]
    public class CannonConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Cannon";
        [field: SerializeField] public CannonballConfig CannonballConfig { get; private set; }
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 1.5f;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 2.5f;
        [field: SerializeField, Min(0)] public float MaxAttackDistance { get; private set; } = 5f;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 150;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1;
        [field: SerializeField] public Teams Team { get; private set; } = Teams.Enemies;
    }
}