using Assets._Project.Develop.Runtime.Infrastracture.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Infrastracture.Gameplay.Infrastracture
{
    public class GameplayContextRegistrations
    {
        public static void Process(DIContainer container, GameplayInputArgs args)
        {
            Debug.Log("Процесс регистрации сервисов на сцене геймплея");
        }
    }
}
