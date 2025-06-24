using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.AssetsManagement
{
    public class ResourcesAssetsLoader
    {
        private ConfigsProviderService _configsProviderService;

        public ResourcesAssetsLoader(ConfigsProviderService configsProviderService)
        {
            _configsProviderService = configsProviderService;
        }

        public T Load<T>(string resoursePath) where T : Object
            => Resources.Load<T>(resoursePath);
    }
}