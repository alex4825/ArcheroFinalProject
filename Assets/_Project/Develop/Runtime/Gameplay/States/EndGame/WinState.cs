using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
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
        private readonly StatsService _statsService;
        private LevelConfig _levelConfig;

        public WinState(
            IInputService inputService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            VictoryDefeatCounter victoryDefeatCounter,
            WalletService walletService,
            GameplayPopupService gameplayPopupService,
            StatsService statsService,
            LevelConfig levelConfig) : base(inputService)
        {
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _victoryDefeatCounter = victoryDefeatCounter;
            _walletService = walletService;
            _gameplayPopupService = gameplayPopupService;
            _statsService = statsService;
            _levelConfig = levelConfig;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Победа!");

            int goldCount = _levelConfig.VictoryGoldCost + (int)(_levelConfig.VictoryGoldCost * (_statsService.GetKoefBy(StatTypes.LootIncrease) - 1));
            int diamondCount = _levelConfig.VictoryDiamondCost + (int)(_levelConfig.VictoryDiamondCost * (_statsService.GetKoefBy(StatTypes.LootIncrease) - 1));

            _victoryDefeatCounter.AddVictory();
            _walletService.Add(CurrencyTypes.Gold, goldCount);
            _walletService.Add(CurrencyTypes.Diamond, diamondCount);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
            _gameplayPopupService.OpenWinPopup();
        }

        public void Update(float deltaTime)
        {

        }
    }
}