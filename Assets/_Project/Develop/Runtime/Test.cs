using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime
{
    public class Test : MonoBehaviour
    {
        private DIContainer _container;

        private void Awake()
        {
            _container = new();

            _container.RegisterAsSingle<ICoroutinesPerformer>(CreateCoroutinesPerformer);
            _container.RegisterAsSingle(CreateConfigsProviderService);
            _container.RegisterAsSingle(CreateResourcesAssetsLoader);

            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            coroutinesPerformer.StartPerform(LoadConfigs());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();

                TestConfig config = configsProviderService.GetConfig<TestConfig>();
                Debug.Log(config.Damage);
            }
        }

        private ConfigsProviderService CreateConfigsProviderService(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private ResourcesAssetsLoader CreateResourcesAssetsLoader(DIContainer c) => new ResourcesAssetsLoader(); 

        private CoroutinesPerformer CreateCoroutinesPerformer(DIContainer c)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = c.Resolve<ResourcesAssetsLoader>();

            CoroutinesPerformer coroutinesPerformerPrefab = resourcesAssetsLoader.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");

            return Instantiate(coroutinesPerformerPrefab);
        }

        private IEnumerator LoadConfigs()
        {
            ConfigsProviderService configsProviderService = _container.Resolve<ConfigsProviderService>();

            Debug.Log("Start load configs");
            yield return configsProviderService.LoadAcync();
            Debug.Log("End load configs");
        }
    }
}