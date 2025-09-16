using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WaveGenerationState : State, IUpdatableState
    {
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly EntitiesBrainsFactory _enemiesFactory;
        private readonly LevelConfig _levelConfig;
        private readonly FortressHolderService _fortressHolderService;

        private Wave _currentWave;
        private int _currentWaveIndex;

        public WaveGenerationState(
            GameplayWaveContext gameplayWaveContext,
            EntitiesBrainsFactory enemiesFactory,
            LevelConfig levelConfig,
            FortressHolderService fortressHolderService)
        {
            _gameplayWaveContext = gameplayWaveContext;
            _enemiesFactory = enemiesFactory;
            _levelConfig = levelConfig;

            _currentWaveIndex = 0;
            _fortressHolderService = fortressHolderService;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Генерация врагов: ВХОД");

            _currentWave = new Wave(_enemiesFactory, _levelConfig, _currentWaveIndex, _fortressHolderService.Fortress);
            _gameplayWaveContext.Set(_currentWave);
            _currentWave.Run();
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Генерация врагов: ВЫХОД");

            _currentWave?.Dispose();

            _currentWaveIndex++;
        }
    }
}