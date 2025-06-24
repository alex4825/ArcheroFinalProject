using Assets._Project.Develop.Runtime.Utilities.AssetsManagement;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime
{
    public class Test : MonoBehaviour
    {
        private ResourcesAssetsLoader _resourcesAssetsLoader;

        private ICoroutinesPerformer _coroutinesPerformer;

        private ConfigsProviderService _configsProviderService;

        private void Awake()
        {
            _resourcesAssetsLoader = CreateResourcesAssetsLoader();

            _coroutinesPerformer = CreateCoroutinesPerformer();

            _configsProviderService = CreateConfigsProviderService();

            _coroutinesPerformer.StartPerform(LoadConfigs());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TestConfig config = _configsProviderService.GetConfig<TestConfig>();
                Debug.Log(config.Damage);
            }
        }

        private ConfigsProviderService CreateConfigsProviderService()
        {
            ResourcesConfigsLoader resourcesConfigsLoader = new ResourcesConfigsLoader(_resourcesAssetsLoader);

            return new ConfigsProviderService(resourcesConfigsLoader);
        }

        private ResourcesAssetsLoader CreateResourcesAssetsLoader() => new ResourcesAssetsLoader();

        private CoroutinesPerformer CreateCoroutinesPerformer()
        {
            CoroutinesPerformer coroutinesPerformerPrefab = _resourcesAssetsLoader.Load<CoroutinesPerformer>("Utilities/CoroutinesPerformer");
            return Instantiate(coroutinesPerformerPrefab);
        }

        private IEnumerator LoadConfigs()
        {
            Debug.Log("Start load configs");
            yield return _configsProviderService.LoadAcync();
            Debug.Log("End load configs");
        }
    }
}