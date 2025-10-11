using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        public event Action CloseButtonClicked;

        [field: SerializeField] public IconTextListView TopBarView { get; private set; }
        [field: SerializeField] public DefendersIconsListView DefenderIconListView { get; private set; }
        [field: SerializeField] public EntitiesHealthDisplay EntitiesHealthDisplay { get; private set; }

        public void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();
    }
}