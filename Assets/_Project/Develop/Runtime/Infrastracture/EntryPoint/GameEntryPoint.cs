using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagement;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.LoadingScreen;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.EntryPoint
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Старт проекта, сетап настроек");
            SetupAppSettings();

            Debug.Log("Процесс регистрации сервисов всего проекта");
            DIContainer container = new DIContainer();

            EntryPointRegistrations.Process(container);

            container.Resolve<ICoroutinesPerformer>().StartPerform(Initialize(container));
        }

        public IEnumerator Initialize(DIContainer container)
        {
            ILoadingScreen loadingScreen = container.Resolve<ILoadingScreen>();

            loadingScreen.Show();

            Debug.Log("Начинается инициализация сервисов");

            yield return container.Resolve<ConfigsProviderService>().LoadAcync();

            yield return new WaitForSeconds(1);

            Debug.Log("Завершается инициализация сервисов");

            loadingScreen.Hide(); 

            Debug.Log("Начинается переход на какую-то сцену");

        }

        private void SetupAppSettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
