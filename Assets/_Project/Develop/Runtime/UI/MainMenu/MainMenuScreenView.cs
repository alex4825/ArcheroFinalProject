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
            WalletView.gameObject.SetActive(true);
            VictoryView.gameObject.SetActive(true);
            DefeatView.gameObject.SetActive(true);

            _playRandomLevelButton.gameObject.SetActive(true);
            _upgradeButton.gameObject.SetActive(true);
            _closeButton.gameObject.SetActive(true);
        }

        public void HideInterface()
        {
            WalletView.gameObject.SetActive(false);
            VictoryView.gameObject.SetActive(false);
            DefeatView.gameObject.SetActive(false);

            _playRandomLevelButton.gameObject.SetActive(false);
            _upgradeButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
        }

        private void OnPlayButtonClicked() => PlayRandomLevelButtonClicked?.Invoke();
        private void OnUpgradeButtonClicked() => UpgradeButtonClicked?.Invoke();
        private void OnCloseButtonClicked() => CloseButtonClicked?.Invoke();
    }
}
