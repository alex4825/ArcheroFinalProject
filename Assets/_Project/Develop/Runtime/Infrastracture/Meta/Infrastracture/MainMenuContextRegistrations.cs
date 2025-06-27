using Assets._Project.Develop.Runtime.Infrastracture.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Meta.Infrastracture
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Процесс регистрации сервисов на сцене главного меню");
        }
    }
}
