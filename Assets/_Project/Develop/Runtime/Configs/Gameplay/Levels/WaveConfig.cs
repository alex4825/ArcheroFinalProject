using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Configs/Gameplay/Levels/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] private List<EntityConfig> _enemyConfigs;

        public IReadOnlyList<EntityConfig> EnemyConfigs => _enemyConfigs;

        [field: SerializeField] public QueueModes EnemiesGenerationMode { get; private set; }
        [field: SerializeField] public float MinSpawnDelayTime { get; private set; } = 2;
        [field: SerializeField] public float MaxSpawnDelayTime { get; private set; } = 3;
        [field: SerializeField] public float MinSpawnRadius { get; private set; } = 5;
        [field: SerializeField] public float MaxSpawnRadius { get; private set; } = 10;
    }
}
