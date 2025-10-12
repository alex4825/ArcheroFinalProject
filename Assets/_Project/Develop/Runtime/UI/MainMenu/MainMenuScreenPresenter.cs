using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Statistics;
using Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly MainMenuPopupService _mainMenuPopupService;
        private int _levelsCount;
        private readonly SoundLauncher _soundLauncher;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            MainMenuPopupService mainMenuPopupService,
            int levelsCount,
            SoundLauncher soundLauncher)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _mainMenuPopupService = mainMenuPopupService;
            _levelsCount = levelsCount;
            _soundLauncher = soundLauncher;
        }

        public void Initialize()
        {
            _screen.PlayRandomLevelButtonClicked += OnPlayRandomLevelButtonClicked;
            _screen.UpgradeButtonClicked += OnUpgradeButtonClicked;
            _screen.CloseButtonClicked += OnCloseButtonClicked;

            CreateWallet();
            CreateDefeatPresenter();
            CreateVictoryPresenter();

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Initialize();
        }

        public void Dispose()
        {
            _screen.PlayRandomLevelButtonClicked -= OnPlayRandomLevelButtonClicked;
            _screen.CloseButtonClicked -= OnCloseButtonClicked;

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);
            _childPresenters.Add(walletPresenter);
        }

        private void CreateDefeatPresenter()
        {
            DefeatPresenter defeatPresenter = _projectPresentersFactory.CreateDefeatPresenter(_screen.DefeatView);
            _childPresenters.Add(defeatPresenter);
        }

        private void CreateVictoryPresenter()
        {
            VictoryPresenter victoryPresenter = _projectPresentersFactory.CreateVictoryPresenter(_screen.VictoryView);
            _childPresenters.Add(victoryPresenter);
        }

        private void OnPlayRandomLevelButtonClicked()
        {
            _soundLauncher.PlayClickSound();

            _coroutinesPerformer.StartPerform(
                _sceneSwitcherService.ProcessSwitchTo
                (Scenes.Gameplay,
                new GameplayInputArgs(Random.Range(1, _levelsCount + 1))));
        }

        private void OnUpgradeButtonClicked()
        {
            _soundLauncher.PlayClickSound();

            UpgradePopupPresenter upgradePopup = _mainMenuPopupService.OpenUpgradePopup();

            upgradePopup.CloseRequest += OnCloseUpgradePopup;

            _screen.HideInterface();
        }

        private void OnCloseUpgradePopup(PopupPresenterBase upgradePopup)
        {
            upgradePopup.CloseRequest -= OnCloseUpgradePopup;
            _screen.ShowInterface();
        }

        private void OnCloseButtonClicked()
        {
            _soundLauncher.PlayClickSound();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
