using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
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
        private readonly GameplayInputArgs _gameplayInputArgs;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly VictoryDefeatCounter _victoryDefeatCounter;
        private readonly WalletService _walletService;
        private int _victoryCost;

        public WinState(
            IInputService inputService,
            PlayerDataProvider playerDataProvider,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            VictoryDefeatCounter victoryDefeatCounter,
            WalletService walletService,
            int victoryCost) : base(inputService)
        {
            _playerDataProvider = playerDataProvider;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _victoryDefeatCounter = victoryDefeatCounter;
            _walletService = walletService;
            _victoryCost = victoryCost;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Победа!");

            _victoryDefeatCounter.AddVictory();
            _walletService.Add(CurrencyTypes.Gold, _victoryCost);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcesSwitchTo(Scenes.MainMenu));
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}