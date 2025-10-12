using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Infrastracture.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Meta.Features.Upgrade;
using Assets._Project.Develop.Runtime.Meta.Sound;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataRepository;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.KeysStorage;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.Serializers;
using Assets._Project.Develop.Runtime.Utilities.DataManipulation;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Infrastracture.EntryPoint
{
    public class ProjectContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);

            container.RegisterAsSingle(CreateSoundLauncher).NonLazy();

            container.RegisterAsSingle(CreateConfigsProviderService);

            container.RegisterAsSingle(CreateResourcesAssetsLoader);

            container.RegisterAsSingle(CreateSceneLoderService);

            container.RegisterAsSingle<ILoadingScreen>(CreateLoadingScreen);

            container.RegisterAsSingle(CreateSceneSwitcherService);

            container.RegisterAsSingle(CreateWalletService).NonLazy();

            container.RegisterAsSingle<ISaveLoadService>(CreateSaveLoadService);

            container.RegisterAsSingle(CreatePlayerDataProvider);

            container.RegisterAsSingle(CreateProjectPresentersFactory);

            container.RegisterAsSingle(CreateViewsFactory);

            container.RegisterAsSingle(CreateLevelsProgressionService).NonLazy();

            container.RegisterAsSingle(CreateTimerService);

            container.RegisterAsSingle(CreateVictoryDefeatCounter).NonLazy();

            container.RegisterAsSingle(CreateStatsService).NonLazy();
        }

        private static SoundLauncher CreateSoundLauncher(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            SoundLauncher SoundLauncherPrefab = resourcesAssetsLoader.Load<SoundLauncher>("Utilities/SoundLauncher");

            return Object.Instantiate(SoundLauncherPrefab);
        }

        private static StatsService CreateStatsService(DIContainer container)
        {
            return new StatsService(
                container.Resolve<PlayerDataProvider>(), 
                container.Resolve<ConfigsProviderService>(),
                container.Resolve<ICoroutinesPerformer>());
        }

        private static VictoryDefeatCounter CreateVictoryDefeatCounter(DIContainer container)
            => new VictoryDefeatCounter(container.Resolve<PlayerDataProvider>());

        private static TimerServiceFactory CreateTimerService(DIContainer container)
            => new TimerServiceFactory(container);

        private static LevelsProgressionService CreateLevelsProgressionService(DIContainer container)
            => new LevelsProgressionService(container.Resolve<PlayerDataProvider>());

        private static ViewsFactory CreateViewsFactory(DIContainer container)
            => new ViewsFactory(container.Resolve<ResourcesAssetsLoader>());

        private static ProjectPresentersFactory CreateProjectPresentersFactory(DIContainer container)
            => new ProjectPresentersFactory(container);

        private static PlayerDataProvider CreatePlayerDataProvider(DIContainer container)
            => new PlayerDataProvider(container.Resolve<ISaveLoadService>(), container.Resolve<ConfigsProviderService>());

        private static SaveLoadService CreateSaveLoadService(DIContainer container)
        {
            IDataSerializer serializer = new JsonSerializer();
            IDataKeysStorage dataKeysStorage = new MapDataKeysStorage();

            IDataRepository dataRepository;
#if UNITY_WEBGL
            dataRepository = new PlayerPrefsDataRepository();
#else
            string saveFolderPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            dataRepository = new LocalFileDataRepository(saveFolderPath, "json");
#endif
            return new SaveLoadService(serializer, dataKeysStorage, dataRepository);
        }

        private static WalletService CreateWalletService(DIContainer container)
        {
            Dictionary<CurrencyTypes, ReactiveVariable<int>> currencies = new();

            foreach (CurrencyTypes type in Enum.GetValues(typeof(CurrencyTypes)))
                currencies[type] = new ReactiveVariable<int>();

            return new WalletService(currencies, container.Resolve<PlayerDataProvider>());
        }

        private static SceneSwitcherService CreateSceneSwitcherService(DIContainer c)
            => new SceneSwitcherService(
                c.Resolve<SceneLoaderService>(),
                c.Resolve<ILoadingScreen>(),
                c
                );

        private static SceneLoaderService CreateSceneLoderService(DIContainer c) => new SceneLoaderService();

        private static ConfigsProviderService CreateConfigsProviderService(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private static ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) => new ResourcesAssetsLoader();

        private static CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            CoroutinesPerformer coroutinesPerformerPrefab = resourcesAssetsLoader.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Object.Instantiate(coroutinesPerformerPrefab);
        }

        private static StandardLoadingScreen CreateLoadingScreen(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            StandardLoadingScreen loadingScreenPrefab = resourcesAssetsLoader.Load<StandardLoadingScreen>("Utilities/StandardLoadingScreen");

            return Object.Instantiate(loadingScreenPrefab);
        }
    }
}
