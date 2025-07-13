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

        private ReactiveVariable<int> _field;
        private ReactiveVariable<int> _field2;
        private List<IDisposable> _disposables = new List<IDisposable>();

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

            _field = new ReactiveVariable<int>(5);
            _field2 = new ReactiveVariable<int>(10);
            _disposables.Add(_field.Subscribe(OnFieldChanged));
            _disposables.Add(_field2.Subscribe((arg1, arg2) => Debug.Log($"Поле изменилось. Старое значение - {arg1}, новое - {arg2}")));
        }

        private void OnFieldChanged(int arg1, int arg2)
        {
            Debug.Log($"Поле изменилось. Старое значение - {arg1}, новое - {arg2}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcesSwitchTo(Scenes.Gameplay, new GameplayInputArgs(2)));
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _field.Value++;
                _field2.Value++;

                foreach (var disposable in _disposables)
                {
                    disposable.Dispose();
                }

                _disposables.Clear();
            }
        }
    }
}
