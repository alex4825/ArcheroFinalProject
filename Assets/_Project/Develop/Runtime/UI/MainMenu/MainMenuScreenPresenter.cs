using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Statistics;
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
        private int _levelsCount;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            int levelsCount)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _levelsCount = levelsCount;
        }

        public void Initialize()
        {
            _screen.PlayRandomLevelButtonClicked += OnPlayRandomLevelButtonClicked;

            CreateWallet();
            CreateDefeatPresenter();
            CreateVictoryPresenter();

            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Initialize();
        }

        public void Dispose()
        {
            _screen.PlayRandomLevelButtonClicked -= OnPlayRandomLevelButtonClicked;

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
            _coroutinesPerformer.StartPerform(
                _sceneSwitcherService.ProcesSwitchTo
                (Scenes.Gameplay,
                new GameplayInputArgs(Random.Range(1, _levelsCount + 1))));
        }
    }
}
