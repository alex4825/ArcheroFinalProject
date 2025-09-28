using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Statistics;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using System.Collections.Generic;
using System;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _view; 
        private readonly GameplayPresentersFactory _gameplayPresentersFactory;         
        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;
        private readonly GameplayWaveContext  _gameplayWaveContext;
        private readonly MainHeroHolderService _mainHeroHolderService;

        private int _wavesCount;

        private readonly List<IPresenter> _childPresenters = new();

        public GameplayScreenPresenter(
            GameplayScreenView view,
            GameplayWaveContext gameplayWaveContext,
            GameplayPresentersFactory gameplayPresentersFactory,
            MainHeroHolderService mainHeroHolderService,
            int wavesCount)
        {
            _view = view;
            _gameplayWaveContext = gameplayWaveContext;
            _gameplayPresentersFactory = gameplayPresentersFactory;
            _mainHeroHolderService = mainHeroHolderService;
            _wavesCount = wavesCount;
        }

        public void Initialize()
        {
            CreateWaveCountPresenter();

            CreateEntitiesHealthDisplay();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
        }

        public void Dispose()
        {
            foreach (IPresenter childPresenter in _childPresenters)
                childPresenter.Dispose();

            _childPresenters.Clear();
        }

        private void CreateWaveCountPresenter()
        {
            _childPresenters.Add(new WaveCountPresenter(_gameplayWaveContext, _view.WaveCountView, _wavesCount));
        }

        private void CreateEntitiesHealthDisplay()
        {
            _entitiesHealthDisplayPresenter = _gameplayPresentersFactory.CreateEntitiesHealthDisplayPresenter(_view.EntitiesHealthDisplay);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }
    }
}