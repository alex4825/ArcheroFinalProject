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
        public event Action CloseButtonClicked;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public IconTextView VictoryView { get; private set; }
        [field: SerializeField] public IconTextView DefeatView { get; private set; }

        [SerializeField] private Button _playRandomLevelButton;
        [SerializeField] private Button _closeButtonClicked;

        private void OnEnable()
        {
            _playRandomLevelButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
            _closeButtonClicked.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnDisable()
        {
            _playRandomLevelButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
            _closeButtonClicked.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnOpenLevelsMenuButtonClicked() => PlayRandomLevelButtonClicked?.Invoke();
        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();
    }
}
