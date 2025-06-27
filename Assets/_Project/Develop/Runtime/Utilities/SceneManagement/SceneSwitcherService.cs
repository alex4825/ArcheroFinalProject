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
        private DIContainer _projectContainer;

        public SceneSwitcherService(SceneLoaderService sceneLoaderService, ILoadingScreen loadingScreen, DIContainer projectContainer)
        {
            _sceneLoaderService = sceneLoaderService;
            _loadingScreen = loadingScreen;
            _projectContainer = projectContainer;
        }

        public IEnumerator ProcesSwitchTo(string sceneName, IInputSceneArgs sceneArgs = null)
        {
            _loadingScreen.Show();

            yield return _sceneLoaderService.LoadAcync(Scenes.Empty);
            yield return _sceneLoaderService.LoadAcync(sceneName);

            SceneBootsprap sceneBootsprap = Object.FindObjectOfType<SceneBootsprap>();

            if(sceneBootsprap ==  null)
                throw new NullReferenceException(nameof(sceneBootsprap) + " not found");

            DIContainer sceneContainer = new DIContainer(_projectContainer);

            sceneBootsprap.ProcessRegistrations(sceneContainer, sceneArgs);

            yield return sceneBootsprap.Initialize();

            _loadingScreen.Hide();

            sceneBootsprap.Run(); 
        }
    }
}
