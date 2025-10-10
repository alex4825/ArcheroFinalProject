using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Gameplay.States.EndGame;
using Assets._Project.Develop.Runtime.Gameplay.Waves;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.DefendersIcons;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly EnemiesFactory _entitiesBrainsFactory;
        private readonly GameplayWaveContext _gameplayWaveContext;
        private readonly AIBrainsContext _brainsContext;
        private readonly FortressHolderService _fortressHolderService;
        private readonly StatsService _statsService;
        private readonly LevelConfig _levelConfig;

        public GameplayStatesFactory(DIContainer container, LevelConfig levelConfig)
        {
            _container = container;
            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _entitiesBrainsFactory = container.Resolve<EnemiesFactory>();
            _gameplayWaveContext = _container.Resolve<GameplayWaveContext>();
            _brainsContext = _container.Resolve<AIBrainsContext>();
            _fortressHolderService = _container.Resolve<FortressHolderService>();
            _statsService = _container.Resolve<StatsService>();
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
                _levelConfig);
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

            ICompositeCondition coreLoopToWinStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => _gameplayWaveContext.WavesPassed == _levelConfig.WavesCount))
                .Add(new FuncCondition(() => _fortressHolderService.Fortress.IsDead.Value == false));

            ICompositeCondition coreLoopToDefeatStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => _fortressHolderService.Fortress == null))
                .Add(new FuncCondition(() => _fortressHolderService.Fortress.IsDead.Value));

            GameplayStateMachine gameplayCycle = new GameplayStateMachine(
                new List<IDisposable> {
                    coreLoopState,
                    coreLoopState.Entered.Subscribe(_brainsContext.Enable),
                    coreLoopState.Exited.Subscribe(_brainsContext.Disable)
                });

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
            WaitingForPointingState waitingForPointState = new WaitingForPointingState(_container.Resolve<IInputService>());

            PlacementDefendersState placementDefendersState = new PlacementDefendersState(
                _container.Resolve<GameplayScreenPresenter>().GetChild<DefendersIconsPresenter>(),
                waitingForPointState.PointFound,
                _entitiesBrainsFactory,
                _container.Resolve<WalletService>());

            entityCreated = placementDefendersState.Created;

            return new GameplayParallelState(waitingForPointState, placementDefendersState);
        }

        private GameplayParallelState CreateWaveCycleState(List<IDisposable> disposables)
        {
            WaveGenerationState waveGenerationState = new WaveGenerationState(
                _gameplayWaveContext,
                _container.Resolve<EnemiesFactory>(),
                _levelConfig,
                _fortressHolderService);

            WaitingForPointingState waitingForExplodePointState = new WaitingForPointingState(_container.Resolve<IInputService>());

            PlayerConfig playerConfig = _container.Resolve<ConfigsProviderService>().GetConfig<PlayerConfig>();

            ReactiveEvent<Vector3> explodedEvent = new();
            ReactiveVariable<float> explodeRadius = new ReactiveVariable<float>(playerConfig.ExplodeRadius);

            Exploder exploder = new(
                _container.Resolve<CollidersRegistryService>(),
                explodeRadius,
                new ReactiveVariable<float>(playerConfig.ExplodeDamage * _statsService.GetKoefBy(StatTypes.ClickDamageIncrease)),
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