using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States.EndGame
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly VictoryDefeatCounter _victoryDefeatCounter;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly GameplayPopupService _gameplayPopupService;

        public DefeatState(
            IInputService inputService,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            VictoryDefeatCounter victoryDefeatCounter,
            GameplayPopupService gameplayPopupService) : base(inputService)
        {
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _victoryDefeatCounter = victoryDefeatCounter;
            _gameplayPopupService = gameplayPopupService;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Поражение!");

            _victoryDefeatCounter.AddDefeat();

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAcync());
            _gameplayPopupService.OpenDefeatPopup();
        }

        public void Update(float deltaTime)
        {

        }
    }
}