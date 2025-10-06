using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders
{
    public abstract class DefenderConfig : EntityConfig
    {
        [field: SerializeField, Min(0)] public int Cost { get; private set; } = 15;
        [field: SerializeField] public Teams Team { get; private set; } = Teams.MainHero;
    }
}