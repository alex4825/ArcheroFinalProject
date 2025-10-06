using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/FlamingPool", fileName = "FlamingPool")]
    public class FlamingPoolConfig : DefenderConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/FlamingPool";
        [field: SerializeField, Min(0)] public float Damage { get; private set; } = 10;
        [field: SerializeField, Min(0)] public float TimeToDealDamage { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1.5f;
    }
}