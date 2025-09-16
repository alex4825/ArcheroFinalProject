using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/ExplodyConfig", fileName = "ExplodyConfig")]
    public class ExplodyConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Explody";
        [field: SerializeField, Min(0)] public float MoveSpeed { get; private set; } = 2;
        [field: SerializeField, Min(0)] public float RotationSpeed { get; private set; } = 900;
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1;
    }
}