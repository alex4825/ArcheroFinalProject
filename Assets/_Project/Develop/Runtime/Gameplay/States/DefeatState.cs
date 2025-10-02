using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.PauseFeature;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        private readonly GameplayPopupService _gameplayPopupService;
        private readonly IPauseService _pauseService;

        public DefeatState(
            IInputService inputService,
            GameplayPopupService gameplayPopupService,
            IPauseService pauseService) : base(inputService, pauseService)
        {
            _gameplayPopupService = gameplayPopupService;
            _pauseService = pauseService;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log("Поражение!");

            _gameplayPopupService.OpenDefeatPopup();
        }

        public void Update(float deltaTime)
        {

        }
    }
}