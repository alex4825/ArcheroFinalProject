using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PlacementDefendersState : State, IUpdatableState
    {
        private DefendersIconsPresenter _defendersIconsPresenter;
        private IReadonlyEvent<Vector3> _placeFound;
        private EnemiesFactory _enemiesFactory;
        private WalletService _walletService;

        private DefenderConfig _currentDefenderConfig;

        private ReactiveEvent<Entity, int> _created = new();

        private IDisposable _placeFoundDisposable;

        public PlacementDefendersState(
            DefendersIconsPresenter defendersIconsPresenter,
            IReadonlyEvent<Vector3> placeFound,
            EnemiesFactory entitiesBrainsFactory,
            WalletService walletService)
        {
            _defendersIconsPresenter = defendersIconsPresenter;
            _placeFound = placeFound;
            _enemiesFactory = entitiesBrainsFactory;
            _walletService = walletService;
        }

        public IReadonlyEvent<Entity, int> Created => _created;

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Расстановка защитников: ВХОД");

            _placeFoundDisposable = _placeFound.Subscribe(OnPlaceFound);
            _defendersIconsPresenter.IconClicked += OnDefenderConfigSelected;
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Расстановка защитников: ВЫХОД");

            _placeFoundDisposable?.Dispose();
            _defendersIconsPresenter.IconClicked -= OnDefenderConfigSelected;
        }

        private void OnPlaceFound(Vector3 position)
        {
            if (_currentDefenderConfig == null)
                return;

            if (_walletService.GetCurrency(CurrencyTypes.Gold).Value > _currentDefenderConfig.Cost)
            {
                Entity entity = _enemiesFactory.Create(position, _currentDefenderConfig, _currentDefenderConfig.Team);
                _created?.Invoke(entity, _currentDefenderConfig.Cost);
                _walletService.Spend(CurrencyTypes.Gold, _currentDefenderConfig.Cost);
            }
        }

        private void OnDefenderConfigSelected(DefenderConfig config)
            => _currentDefenderConfig = config;
    }
}