using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Configs.Gameplay.Entities
{
    [CreateAssetMenu(menuName = "Configs/Gameplay/Entities/PlayerConfig", fileName = "PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        //[field: SerializeField] public string PrefabPath { get; private set; } = "Entities/Player";
        [field: SerializeField, Min(0)] public float ExplodeDamage { get; private set; } = 30;
        [field: SerializeField, Min(0)] public float ExplodeRadius { get; private set; } = 5;
        [field: SerializeField] public Teams Team { get; private set; } = Teams.MainHero;
    }
}