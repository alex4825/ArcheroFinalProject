using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.States.EndGame;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System.Collections.Generic;
using System;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Assets._Project.Develop.Runtime.Utilities;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly LevelConfig _levelConfig;

        public GameplayStatesFactory(DIContainer container, LevelConfig levelConfig)
        {
            _container = container;
            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _gameplayWaveContext = _container.Resolve<GameplayWaveContext>();
            _levelConfig = levelConfig;
        }

        public WinState CreateWinState()
        {
            return new WinState(
                _container.Resolve<IInputService>(),
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<VictoryDefeatCounter>());
        }

        public DefeatState CreateDefeatState()
        {
            return new DefeatState(
                _container.Resolve<IInputService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<VictoryDefeatCounter>());
        }

        public GameplayStateMachine CreateGameplayStateMachine()
        {
            GameplayStateMachine coreLoopState = CreateCoreLoopState(_levelConfig.DelayBetweenWaves);

            DefeatState defeatState = CreateDefeatState();
            WinState winState = CreateWinState();

            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();
            FortressHolderService fortressHolderService = _container.Resolve<FortressHolderService>();

            FuncCondition coreLoopToWinStateCondition = new FuncCondition(() => _gameplayWaveContext.WavesCount == _levelConfig.WavesCount);

            ICompositeCondition coreLoopToDefeatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    if (fortressHolderService.Fortress != null)
                        return fortressHolderService.Fortress.IsDead.Value;

                    return false;
                }));

            GameplayStateMachine gameplayCycle = new GameplayStateMachine(new List<IDisposable> { coreLoopState });

            gameplayCycle.AddState(coreLoopState);
            gameplayCycle.AddState(winState);
            gameplayCycle.AddState(defeatState);

            gameplayCycle.AddTransition(coreLoopState, winState, coreLoopToWinStateCondition);
            gameplayCycle.AddTransition(coreLoopState, defeatState, coreLoopToDefeatStateCondition);

            return gameplayCycle;
        }

        public GameplayStateMachine CreateCoreLoopState(float startDelayTime)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            TimerService startDelayTimer = _timerServiceFactory.Create(startDelayTime);

            PlacementMinesState placementMinesState = new PlacementMinesState();

            GameplayParallelState waveCycleState = CreateWaveCycleState();

            bool isWaveWin = false;

            disposables.Add(startDelayTimer);

            disposables.Add(placementMinesState.Entered.Subscribe(() => startDelayTimer.Restart()));

            disposables.Add(waveCycleState.Entered.Subscribe(() => isWaveWin = false));

            disposables.Add(_gameplayWaveContext.CurrentWaveEnded.Subscribe((waveResult) => isWaveWin = waveResult.IsWin));

            _gameplayWaveContext.CurrentWaveEnded.Subscribe((waveResult) => isWaveWin = waveResult.IsWin);

            FuncCondition placementMinesToWaveCycleCondition = new FuncCondition(() => startDelayTimer.IsOver);
            FuncCondition waveCycleToPlacementMinesCondition = new FuncCondition(() => isWaveWin);

            GameplayStateMachine coreLoopState = new GameplayStateMachine(disposables);

            coreLoopState.AddState(waveCycleState);
            coreLoopState.AddState(placementMinesState);

            coreLoopState.AddTransition(placementMinesState, waveCycleState, placementMinesToWaveCycleCondition);
            coreLoopState.AddTransition(waveCycleState, placementMinesState, waveCycleToPlacementMinesCondition);

            return coreLoopState;
        }

        private GameplayParallelState CreateWaveCycleState()
        {
            List<IDisposable> disposables = new List<IDisposable>();

            WaveGenerationState waveGenerationState = new WaveGenerationState(
                _gameplayWaveContext,
                _container.Resolve<EnemiesFactory>(),
                _levelConfig,
                _container.Resolve<FortressHolderService>());

            WaitingForExplodePointState waitingForExplodePointState = new WaitingForExplodePointState(_container.Resolve<IInputService>());

            bool needExplode = false;
            Vector3 explodePoint = new();

            disposables.Add(waitingForExplodePointState.PointFound.Subscribe(point =>
            {
                needExplode = true;
                explodePoint = point;
            }));

            ExplodeState explodeState = new(
                _container.Resolve<CollidersRegistryService>(),
                () => explodePoint,
                5,
                40,
                Layers.CharactersMask
                );

            GameplayStateMachine explodeBehavior = new GameplayStateMachine(disposables);

            explodeBehavior.AddState(waitingForExplodePointState);
            explodeBehavior.AddState(explodeState);

            explodeBehavior.AddTransition(waitingForExplodePointState, explodeState, new FuncCondition(() => needExplode));
            explodeBehavior.AddTransition(explodeState, waitingForExplodePointState, new FuncCondition(() =>
            {
                needExplode = false;
                return true;
            }));

            return new GameplayParallelState(waveGenerationState, explodeBehavior);
        }
    }
}