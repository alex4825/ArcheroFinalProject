using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States.EndGame
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly VictoryDefeatCounter _victoryDefeatCounter;
        private readonly WalletService _walletService;
        private readonly GameplayPopupService _gameplayPopupService;
        private int _victoryCost;

        public WinState(
            IInputService inputService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            VictoryDefeatCounter victoryDefeatCounter,
            WalletService walletService,
            GameplayPopupService gameplayPopupService,
            int victoryCost) : base(inputService)
        {
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _victoryDefeatCounter = victoryDefeatCounter;
            _walletService = walletService;
            _gameplayPopupService = gameplayPopupService;
            _victoryCost = victoryCost;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Победа!");

            _victoryDefeatCounter.AddVictory();
            _walletService.Add(CurrencyTypes.Gold, _victoryCost);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
            _gameplayPopupService.OpenWinPopup();
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}