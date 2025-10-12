using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Configs.Meta.Upgrade;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.UpgradeMenuPopup;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPresentersFactory
    {
        private readonly DIContainer _container;

        public MainMenuPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public UpgradeCardPresenter CreateUpgradeCardPresenter(UpgradeCardView cardView, UpgradeConfig config)
        {
            return new UpgradeCardPresenter(cardView, config, _container.Resolve<ViewsFactory>(), _container.Resolve<StatsService>());
        }

        public UpgradePopupPresenter CreateUpgradePopupPresenter(UpgradePopupView view)
        {
            return new UpgradePopupPresenter(
                view,
                _container.Resolve<ConfigsProviderService>().GetConfig<StatsConfig>(),
                _container.Resolve<StatsService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<ViewsFactory>(),
                _container.Resolve<MainMenuPresentersFactory>(),
                _container.Resolve<WalletService>(),
                _container.Resolve<SoundLauncher>());
        }

        public MainMenuScreenPresenter CreateMainMenuScreen(MainMenuScreenView view)
        {
            return new MainMenuScreenPresenter(
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<MainMenuPopupService>(),
                _container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().Levels.Count,
                _container.Resolve<SoundLauncher>());
        }
    }
}
