using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Meta.Infrastracture
{
    public class MainMenuBootstrap : SceneBootsprap
    {
        private DIContainer _container;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }
        public override IEnumerator Initialize()
        {
            Debug.Log("Инициализация сцены главного меню.");

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Старт сцены главного меню.");
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcesSwitchTo(Scenes.Gameplay, new GameplayInputArgs(2)));
            }
        }
    }
}
