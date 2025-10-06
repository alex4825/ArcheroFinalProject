using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PlacementDefendersState : State, IUpdatableState
    {
        private DefenderConfig _defenderConfig;
        private IReadonlyEvent<Vector3> _placeFound;
        private EnemiesFactory _enemiesFactory;
        private WalletService _walletService;

        private ReactiveEvent<Entity> _created = new();

        private IDisposable _placeFoundDisposable;

        public PlacementDefendersState(
            DefenderConfig defenderConfig,
            IReadonlyEvent<Vector3> placeFound,
            EnemiesFactory entitiesBrainsFactory,
            WalletService walletService)
        {
            _defenderConfig = defenderConfig;
            _placeFound = placeFound;
            _enemiesFactory = entitiesBrainsFactory;
            _walletService = walletService;
        }

        public IReadonlyEvent<Entity> Created => _created;

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Расстановка защитников: ВХОД");

            _placeFoundDisposable = _placeFound.Subscribe(OnPlaceFound);
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Расстановка защитников: ВЫХОД");

            _placeFoundDisposable?.Dispose();
        }

        private void OnPlaceFound(Vector3 position)
        {
            if (_walletService.GetCurrency(CurrencyTypes.Gold).Value > _defenderConfig.Cost)
            {
                Entity entity = _enemiesFactory.Create(position, _defenderConfig, _defenderConfig.Team);
                _created?.Invoke(entity);
                _walletService.Spend(CurrencyTypes.Gold, _defenderConfig.Cost);
            }
        }
    }
}