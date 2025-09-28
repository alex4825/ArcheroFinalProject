using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using System;
using UnityEngine;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.UI.Gameplay;

namespace Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture
{
    public class GameplayBootstap : SceneBootsprap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;

        private WalletService _walletService;

        private GameplayStatesContext _gameplayStatesContext;
        private EntitiesLifeContext _entitiesLifeContext;
        private AIBrainsContext _brainsContext;

        private GameplayScreenPresenter _screenPresenter;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is GameplayInputArgs gameplayInputArgs)
                _inputArgs = gameplayInputArgs;
            else if (sceneArgs != null)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log($"Вы попали на уровень {_inputArgs.LevelNumber}");

            Debug.Log("Инициализация геймплейной сцены.");

            _walletService = _container.Resolve<WalletService>();

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _brainsContext = _container.Resolve<AIBrainsContext>();
            _gameplayStatesContext = _container.Resolve<GameplayStatesContext>();

            CreateEnvironment();
            
            _screenPresenter = _container.Resolve<GameplayScreenPresenter>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Старт геймплейной сцены.");

            _gameplayStatesContext.Run();
        }

        private void Update()
        {
            _container?.Update(Time.deltaTime);
        }

        private void CreateEnvironment()
        {
            LevelConfig currentLevel = _container.Resolve<ConfigsProviderService>().GetConfig<LevelsListConfig>().GetBy(_inputArgs.LevelNumber);
            LevelEnvironment levelEnvironment = Instantiate(currentLevel.LevelEnvironment);

            _container.Resolve<EntitiesFactory>().CreateFortress(levelEnvironment.Fortress);
        }

        private void LateUpdate()
        {
            _screenPresenter?.LateUpdate();
        }
    }
}
