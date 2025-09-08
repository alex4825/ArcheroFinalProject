using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Configs/Gameplay/Levels/WaveConfig")]
    public class WaveConfig : ScriptableObject
    {
        [SerializeField] private List<SerializableKeyValuePair<EntityConfig, int>> _enemyConfigsToCount;

        public IReadOnlyDictionary<EntityConfig, int> EnemyConfigsToCount => DictionarySerializer.GetFrom(_enemyConfigsToCount);

        [field: SerializeField] public int MinSpawnDurationTime { get; private set; } = 2;
        [field: SerializeField] public int MaxSpawnDurationTime { get; private set; } = 3;
    }
}
