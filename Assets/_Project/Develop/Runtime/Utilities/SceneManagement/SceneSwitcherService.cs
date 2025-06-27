using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _loadingScreen.Hide();
        }
    }
}
