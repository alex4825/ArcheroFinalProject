using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture
{
    public class GameplayBootstap : SceneBootsprap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, gameplayInputArgs);
        }

        public override IEnumerator Initialize()
        {           
            Debug.Log($"Вы попали на уровень {_inputArgs.LevelNumber}");

            Debug.Log("Инициализация геймплейной сцены.");

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Старт геймплейной сцены.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcesSwitchTo(Scenes.MainMenu));
            }
        }
    }
}
