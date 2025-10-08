using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/MineConfig", fileName = "MineConfig")]
    public class MineConfig : DefenderConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Mine";
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float ExplodeDuration { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1;
    }
}