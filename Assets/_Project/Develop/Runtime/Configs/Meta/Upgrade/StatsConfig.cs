using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Meta.Upgrade
{
    [CreateAssetMenu(fileName = "StatsConfig", menuName = "Configs/Meta/Upgrade/StatsConfig")]
    public class StatsConfig : ScriptableObject
    {
        [SerializeField] private List<UpgradeConfig> _values;

        public IReadOnlyList<UpgradeConfig> Configs => _values;

        public UpgradeConfig GetBy(StatTypes upgradeType)
            => _values.First(config => config.Type == upgradeType);
    }

    [Serializable]
    public class UpgradeConfig
    {
        [field: SerializeField] public StatTypes Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Label { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField, Range(0, 1)] public float Koef { get; private set; }
    }


}