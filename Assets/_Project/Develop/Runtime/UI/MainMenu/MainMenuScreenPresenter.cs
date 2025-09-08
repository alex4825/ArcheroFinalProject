using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Statistics;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _screen;

        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(MainMenuScreenView screen, ProjectPresentersFactory projectPresentersFactory)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
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
            //_popupService.OpenLevelsMenuPopup();
        }
    }
}
