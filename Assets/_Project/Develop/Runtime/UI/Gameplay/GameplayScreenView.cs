using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        [field: SerializeField] public IconTextView WaveCountView { get; private set; }
        [field: SerializeField] public EntitiesHealthDisplay EntitiesHealthDisplay { get; private set; }
    }
}