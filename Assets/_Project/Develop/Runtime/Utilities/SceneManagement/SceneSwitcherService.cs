using Assets._Project.Develop.Runtime.Infrastracture;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using System;
using System.Collections;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Utilities.SceneManagement
{
    public class SceneSwitcherService
    {
        private SceneLoaderService _sceneLoaderService;
        private ILoadingScreen _loadingScreen;
        private DIContainer _container;

        public SceneSwitcherService(SceneLoaderService sceneLoaderService, ILoadingScreen loadingScreen, DIContainer container)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
            _container = container;
        }

        public IEnumerator ProcesSwitchTo(string sceneName)
        {
            _loadingScreen.Show();

            yield return _sceneLoaderService.LoadAcync(Scenes.Empty);
            yield return _sceneLoaderService.LoadAcync(sceneName);

            SceneBootsprap sceneBootsprap = Object.FindObjectOfType<SceneBootsprap>();

            if(sceneBootsprap ==  null)
                throw new NullReferenceException(nameof(sceneBootsprap) + " not found");

            yield return sceneBootsprap.Initialize(_container);

            _loadingScreen.Hide();

            sceneBootsprap.Run(); 
        }
    }
}
