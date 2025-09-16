using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PlacementMinesState : State, IUpdatableState
    {
        private MineConfig _mineConfig;
        private IReadonlyEvent<Vector3> _placeFound;
        private EntitiesBrainsFactory _entitiesBrainsFactory;
        private WalletService _walletService;

        private IDisposable _placeFoundDisposable;

        public PlacementMinesState(
            MineConfig mineConfig,
            IReadonlyEvent<Vector3> placeFound,
            EntitiesBrainsFactory entitiesBrainsFactory,
            WalletService walletService)
        {
            _mineConfig = mineConfig;
            _placeFound = placeFound;
            _entitiesBrainsFactory = entitiesBrainsFactory;
            _walletService = walletService;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Расстановка мин: ВХОД");

            _placeFoundDisposable = _placeFound.Subscribe(OnPlaceFound);
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Расстановка мин: ВЫХОД");

            _placeFoundDisposable?.Dispose();
        }

        private void OnPlaceFound(Vector3 position)
        {
            if (_walletService.GetCurrency(CurrencyTypes.Gold).Value > _mineConfig.Cost)
            {
                _entitiesBrainsFactory.Create(position, _mineConfig, Teams.MainHero);
                _walletService.Spend(CurrencyTypes.Gold, _mineConfig.Cost);
            }
        }
    }
}