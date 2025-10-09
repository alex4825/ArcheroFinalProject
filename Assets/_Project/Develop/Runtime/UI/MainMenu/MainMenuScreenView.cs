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
        public event Action UpgradeButtonClicked;
        public event Action CloseButtonClicked;

        [SerializeField] private Button _playRandomLevelButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _closeButton;

        [field: SerializeField] public IconTextListView WalletView { get; private set; }
        [field: SerializeField] public IconTextView VictoryView { get; private set; }
        [field: SerializeField] public IconTextView DefeatView { get; private set; }

        private void OnEnable()
        {
            _playRandomLevelButton.onClick.AddListener(OnPlayButtonClicked);
            _upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        public void ShowInterface()
        {
            WalletView.enabled = true;
            VictoryView.enabled = true;
            DefeatView.enabled = true;

            _playRandomLevelButton.enabled = true;
            _upgradeButton.enabled = true;
            _closeButton.enabled = true;
        }

        public void HideInterface()
        {
            WalletView.enabled = false;
            VictoryView.enabled = false;
            DefeatView.enabled = false;

            _playRandomLevelButton.enabled = false;
            _upgradeButton.enabled = false;
            _closeButton.enabled = false;
        }

        private void OnPlayButtonClicked() => PlayRandomLevelButtonClicked?.Invoke();
        private void OnUpgradeButtonClicked() => UpgradeButtonClicked?.Invoke();
        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();
    }
}
