using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/Gameplay/Levels/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private List<WaveConfig> _waveConfigs;

        [field: SerializeField] public int DelayBetweenWaves { get; private set; } = 5;
        [field: SerializeField] public int FortressHP { get; private set; } = 500;
        [field: SerializeField] public LevelEnvironment LevelEnvironment { get; private set; }

        public Vector3 FortressPosition => LevelEnvironment.Fortress.transform.position;

        public int WavesCount => _waveConfigs.Count;

        public WaveConfig GetWaveConfigBy(int index) => _waveConfigs[index];
    }
}
