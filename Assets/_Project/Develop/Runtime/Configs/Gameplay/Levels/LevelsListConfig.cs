using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "LevelsListConfig", menuName = "Configs/Gameplay/Levels/LevelsListConfig")]
    public class LevelsListConfig : ScriptableObject
    {
        [SerializeField] private List<LevelConfig> _levels;

        public IReadOnlyList<LevelConfig> Levels => _levels;

        public LevelConfig GetBy(int levelNumber) => _levels[levelNumber - 1];
    }
}
