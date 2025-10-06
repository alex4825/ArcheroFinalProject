using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
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
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.UI.Core;
using static UnityEngine.EventSystems.EventTrigger;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly EnemiesFactory _entitiesBrainsFactory;
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly LevelConfig _levelConfig;

        public GameplayStatesFactory(DIContainer container, LevelConfig levelConfig)
        {
            _container = container;
            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _entitiesBrainsFactory = container.Resolve<EnemiesFactory>();
            _gameplayWaveContext = _container.Resolve<GameplayWaveContext>();
            _levelConfig = levelConfig;
        }

        public WinState CreateWinState()
        {
            return new WinState(
                _container.Resolve<IInputService>(),
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<VictoryDefeatCounter>(),
                _container.Resolve<WalletService>(),
                _container.Resolve<GameplayPopupService>(),
                _levelConfig.VictoryCost);
        }

        public DefeatState CreateDefeatState()
        {
            return new DefeatState(
                _container.Resolve<IInputService>(),
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<VictoryDefeatCounter>(),
                _container.Resolve<GameplayPopupService>());
        }

        public GameplayStateMachine CreateGameplayStateMachine()
        {
            GameplayStateMachine coreLoopState = CreateCoreLoopState(_levelConfig.DelayBetweenWaves);

            DefeatState defeatState = CreateDefeatState();
            WinState winState = CreateWinState();

            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();
            FortressHolderService fortressHolderService = _container.Resolve<FortressHolderService>();

            ICompositeCondition coreLoopToWinStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => _gameplayWaveContext.WavesPassed == _levelConfig.WavesCount))
                .Add(new FuncCondition(() => fortressHolderService.Fortress.IsDead.Value == false));

            ICompositeCondition coreLoopToDefeatStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => fortressHolderService.Fortress == null))
                .Add(new FuncCondition(() => fortressHolderService.Fortress.IsDead.Value));

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

            GameplayParallelState restPhaseState = CreateRestPhaseState(out IReadonlyEvent<Entity> entityCreated);

            GameplayParallelState waveCycleState = CreateWaveCycleState(disposables);

            Buffer<Entity> createdDefenders = new(64);
            disposables.Add(entityCreated.Subscribe(entity => createdDefenders.TryAdd(entity)));

            disposables.Add(waveCycleState.Exited.Subscribe(() => KillOneWaveLifetime(createdDefenders)));

            bool isWaveWin = false;

            disposables.Add(startDelayTimer);

            disposables.Add(restPhaseState.Entered.Subscribe(() => startDelayTimer.Restart()));

            disposables.Add(waveCycleState.Entered.Subscribe(() => isWaveWin = false));

            disposables.Add(_gameplayWaveContext.CurrentWaveEnded.Subscribe((waveResult) => isWaveWin = waveResult.IsWin));

            _gameplayWaveContext.CurrentWaveEnded.Subscribe((waveResult) => isWaveWin = waveResult.IsWin);

            FuncCondition placementMinesToWaveCycleCondition = new FuncCondition(() => startDelayTimer.IsOver);
            FuncCondition waveCycleToPlacementMinesCondition = new FuncCondition(() => isWaveWin);

            GameplayStateMachine coreLoopState = new GameplayStateMachine(disposables);

            coreLoopState.AddState(restPhaseState);
            coreLoopState.AddState(waveCycleState);

            coreLoopState.AddTransition(restPhaseState, waveCycleState, placementMinesToWaveCycleCondition);
            coreLoopState.AddTransition(waveCycleState, restPhaseState, waveCycleToPlacementMinesCondition);

            return coreLoopState;
        }

        private void KillOneWaveLifetime(Buffer<Entity> defenders)
        {
            for (int i = defenders.Count - 1; i >= 0; i--)
            {
                if (defenders.Items[i].HasComponent<IsOneWaveLifetime>())
                {
                    defenders.Items[i].IsDead.Value = true;
                    defenders.RemoveItemAt(i);
                }
            }
        }

        private GameplayParallelState CreateRestPhaseState(out IReadonlyEvent<Entity> entityCreated)
        {
            WaitingForPointingState waitingForExplodePointState = new WaitingForPointingState(_container.Resolve<IInputService>());

            PlacementDefendersState placementDefendersState = new PlacementDefendersState(
                _levelConfig.DefenderConfig,
                waitingForExplodePointState.PointFound,
                _entitiesBrainsFactory,
                _container.Resolve<WalletService>());

            entityCreated = placementDefendersState.Created;

            return new GameplayParallelState(waitingForExplodePointState, placementDefendersState);
        }

        private GameplayParallelState CreateWaveCycleState(List<IDisposable> disposables)
        {
            WaveGenerationState waveGenerationState = new WaveGenerationState(
                _gameplayWaveContext,
                _container.Resolve<EnemiesFactory>(),
                _levelConfig,
                _container.Resolve<FortressHolderService>());

            WaitingForPointingState waitingForExplodePointState = new WaitingForPointingState(_container.Resolve<IInputService>());

            PlayerConfig playerConfig = _container.Resolve<ConfigsProviderService>().GetConfig<PlayerConfig>();

            ReactiveEvent<Vector3> explodedEvent = new();
            ReactiveVariable<float> explodeRadius = new ReactiveVariable<float>(playerConfig.ExplodeRadius);

            Exploder exploder = new(
                _container.Resolve<CollidersRegistryService>(),
                explodeRadius,
                new ReactiveVariable<float>(playerConfig.ExplodeDamage),
                Layers.EntityMask,
                new ReactiveVariable<Teams>(Teams.MainHero),
                explodedEvent);

            FreeExploderPresenter freeExploderPresenter = new FreeExploderPresenter(
                _container.Resolve<ViewsFactory>().Create<FreeExploderView>(ViewIDs.FreeExploderView),
                explodedEvent,
                explodeRadius
                );

            disposables.Add(freeExploderPresenter);
            disposables.Add(waitingForExplodePointState.PointFound.Subscribe(exploder.ExplodeIn));

            return new GameplayParallelState(waveGenerationState, waitingForExplodePointState);
        }
    }
}