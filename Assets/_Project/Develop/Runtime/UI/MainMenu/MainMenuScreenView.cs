using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        public event Action PlayRandomLevelButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public IconTextView VictoryView { get; private set; }
        [field: SerializeField] public IconTextView DefeatView { get; private set; }

        [SerializeField] private Button _playRandomLevelButton;

        private void OnEnable()
        {
            _playRandomLevelButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
        }

        private void OnDisable()
        {

            _playRandomLevelButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
        }

        private void OnOpenLevelsMenuButtonClicked() => PlayRandomLevelButtonClicked?.Invoke();
    }
}
