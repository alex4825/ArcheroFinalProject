using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.Serializers;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Meta.Infrastracture
{
    public class MainMenuBootstrap : SceneBootsprap
    {
        private DIContainer _container;

        private WalletService _walletService;

        private PlayerData _playerData;

        private ISaveLoadService _saveLoadService;
        private ICoroutinesPerformer _coroutinesPerformer;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }
        public override IEnumerator Initialize()
        {
            Debug.Log("Инициализация сцены главного меню.");

            _walletService = _container.Resolve<WalletService>();

            _saveLoadService = _container.Resolve<ISaveLoadService>();
            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            _playerData = new PlayerData();
            _playerData.WalletData = new Dictionary<CurrencyTypes, int>
            {
                {CurrencyTypes.Diamond, 10 },
                {CurrencyTypes.Gold, 150 },
            };

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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                Debug.Log($"Золота осталось : {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) 
            {
                if (_walletService.Enough(CurrencyTypes.Gold, 10))
                {
                    _walletService.Spend(CurrencyTypes.Gold, 10);
                    Debug.Log($"Золота осталось : {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _coroutinesPerformer.StartPerform(_saveLoadService.Save(_playerData));
                Debug.Log("Сохранение было вызвано"); 
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _coroutinesPerformer.StartPerform(LoadPlayerData());
            }
        }

        IEnumerator LoadPlayerData()
        {
            PlayerData loadedPlayerData = null;

            yield return _saveLoadService.Load<PlayerData>(result  => loadedPlayerData = result);

            Debug.Log($"Золота загружено: {loadedPlayerData.WalletData[CurrencyTypes.Gold]}");
            Debug.Log($"Алмазов загружено: {loadedPlayerData.WalletData[CurrencyTypes.Diamond]}");
        }
    }
}
