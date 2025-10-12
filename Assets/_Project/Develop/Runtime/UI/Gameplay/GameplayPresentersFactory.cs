using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using Assets._Project.Develop.Runtime.UI.Gameplay.Wave;
using Assets._Project.Develop.Runtime.UI.Statistics;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.Timer;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPresentersFactory
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _gameplayInputArgs;
        private readonly ViewsFactory _viewsFactory;

        public GameplayPresentersFactory(DIContainer container, GameplayInputArgs gameplayInputArgs)
        {
            _container = container;
            _gameplayInputArgs = gameplayInputArgs;
            _viewsFactory = _container.Resolve<ViewsFactory>();
        }

        public RestTimerPresenter CreateRestTimerPresenter(SimpleListView parent, TimerService timer)
        {
            RestTimerView view = _viewsFactory.Create<RestTimerView>(ViewIDs.RestTimerView);
            parent.Add(view);

            return new RestTimerPresenter(view, timer, _viewsFactory);
        }

        public CurrencyPresenter CreateGoldPresenter(SimpleListView parent)
        {
            IconTextView view = _viewsFactory.Create<IconTextView>(ViewIDs.CurrencyView);
            parent.Add(view);

            CurrencyPresenter goldPresenter = _container.Resolve<ProjectPresentersFactory>().CreateCurrencyPresenter(
                view,
                _container.Resolve<WalletService>().GetCurrency(CurrencyTypes.Gold),
                CurrencyTypes.Gold);

            return goldPresenter;
        }

        public WaveCountPresenter CreateWaveCountPresenter(SimpleListView parent, int wavesCount)
        {
            IconTextView view = _viewsFactory.Create<IconTextView>(ViewIDs.WavesCountView);
            parent.Add(view);

            return new WaveCountPresenter(_container.Resolve<GameplayWaveContext>(), view, wavesCount);
        }

        public DefendersIconsPresenter CreateDefendersIconsPresenter(DefendersIconsListView iconsListView)
        {
            return new DefendersIconsPresenter(_viewsFactory, iconsListView);
        }

        public EntitiesHealthDisplayPresenter CreateEntitiesHealthDisplayPresenter(EntitiesHealthDisplay view)
        {
            return new EntitiesHealthDisplayPresenter(
                _container.Resolve<EntitiesLifeContext>(),
                view,
                _viewsFactory,
                this);
        }

        public EntityHealthPresenter CreateEntityHealthPresenter(Entity entity, BarWithText view)
        {
            return new EntityHealthPresenter(entity, view);
        }

        public GameplayScreenPresenter CreateGameplayScreenPresenter(GameplayScreenView view, LevelConfig currentLevelConfig)
        {
            return new GameplayScreenPresenter(
                view,
                _container.Resolve<GameplayWaveContext>(),
                this,
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<SoundLauncher>(),
                currentLevelConfig.WavesCount);
        }

        public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
        {
            return new WinPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<SoundLauncher>());
        }

        public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
        {
            return new DefeatPopupPresenter(
                _container.Resolve<ICoroutinesPerformer>(),
                view,
                _container.Resolve<SceneSwitcherService>(),
                _gameplayInputArgs,
                _container.Resolve<SoundLauncher>());
        }
    }
}