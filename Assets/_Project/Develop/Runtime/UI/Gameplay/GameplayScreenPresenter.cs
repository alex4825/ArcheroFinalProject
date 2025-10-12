using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.Wave;
using Assets._Project.Develop.Runtime.UI.Statistics;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _view;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory;
        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly SoundLauncher _soundLauncher;

        private int _wavesCount;

        private readonly List<IPresenter> _childPresenters = new();
        private IDisposable _cooldownTimerDisposable;

        public GameplayScreenPresenter(
            GameplayScreenView view,
            GameplayWaveContext gameplayWaveContext,
            GameplayPresentersFactory gameplayPresentersFactory,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            SoundLauncher soundLauncher,
            int wavesCount)
        {
            _view = view;
            _gameplayWaveContext = gameplayWaveContext;
            _gameplayPresentersFactory = gameplayPresentersFactory;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _soundLauncher = soundLauncher;
            _wavesCount = wavesCount;
        }

        public void Initialize()
        {
            CreateTopBarPresenters();

            CreateDefendersIconsPresenter();

            CreateEntitiesHealthDisplay();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();

            _view.CloseButtonClicked += OnCloseButtonClicked;
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
        }

        public void Dispose()
        {
            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter?.Dispose();

            _childPresenters.Clear();

            _view.CloseButtonClicked -= OnCloseButtonClicked;
        }

        public TPresenter GetChild<TPresenter>() where TPresenter : class, IPresenter
        {
            return _childPresenters.OfType<TPresenter>().First();
        }

        public void ShowTimer(TimerService timer)
        {
            RestTimerPresenter restTimerPresenter = _gameplayPresentersFactory.CreateRestTimerPresenter(_view.TopBarView, timer);
            restTimerPresenter.Initialize();

            _cooldownTimerDisposable = timer.CooldownEnded.Subscribe(() => HideTimer(restTimerPresenter));

            _childPresenters.Add(restTimerPresenter);
        }

        private void HideTimer(RestTimerPresenter restTimerPresenter)
        {
            _childPresenters.Remove(restTimerPresenter);

            _cooldownTimerDisposable.Dispose();
            restTimerPresenter?.Dispose();
        }

        private void OnCloseButtonClicked()
        {
            _soundLauncher.PlayClickSound();

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
        }

        private void CreateDefendersIconsPresenter()
        {
            _childPresenters.Add(_gameplayPresentersFactory.CreateDefendersIconsPresenter(_view.DefenderIconListView));
        }

        private void CreateTopBarPresenters()
        {
            _childPresenters.Add(_gameplayPresentersFactory.CreateGoldPresenter(_view.TopBarView));

            _childPresenters.Add(_gameplayPresentersFactory.CreateWaveCountPresenter(_view.TopBarView, _wavesCount));
        }

        private void CreateEntitiesHealthDisplay()
        {
            _entitiesHealthDisplayPresenter = _gameplayPresentersFactory.CreateEntitiesHealthDisplayPresenter(_view.EntitiesHealthDisplay);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }
    }
}