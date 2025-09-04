using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        [SerializeField] private HeroConfig _heroConfig;
        [SerializeField] private StageConfig _stageConfig;

        private MainHeroFactory _mainHeroFactory;
        private EnemiesFactory _enemiesFactory;
        private StagesFactory _stagesFactory;
        private IStage _stage;

        private bool _isRunning = false;

        private Entity _hero;
        private Entity _ghost;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _brainsFactory = container.Resolve<BrainsFactory>();

            _mainHeroFactory = container.Resolve<MainHeroFactory>();
            _enemiesFactory = container.Resolve<EnemiesFactory>();
            _stagesFactory = container.Resolve<StagesFactory>();
        }

        public void Run()
        {
            _hero = _mainHeroFactory.Create(Vector3.zero);

            _stage = _stagesFactory.Create(_stageConfig);
            _stage.Completed.Subscribe(OnCompleted);
            _stage.Start();

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            _stage.Update(Time.deltaTime);
        }

        private void OnCompleted()
        {
            Debug.Log("онаедю!");
            _stage.Cleanup();
        }
    }
}
