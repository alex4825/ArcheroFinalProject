using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/MineConfig", fileName = "MineConfig")]
    public class MineConfig : EntityConfig
    {
        [field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Mine";
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 50;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 5;
        [field: SerializeField, Min(0)] public float ExplodeDuration { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float DeathProcessTime { get; private set; } = 1;
        [field: SerializeField, Min(0)] public int Cost { get; private set; } = 15;
        [field: SerializeField] public Teams Team { get; private set; } = Teams.MainHero;
    }
}