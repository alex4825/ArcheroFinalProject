using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        public void Setup(Entity entity)
        {
            MonoEntityRegistrator[] monoEntityRegistrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (monoEntityRegistrators != null)
                foreach (var registrator in monoEntityRegistrators)
                    registrator.Register(entity);
        }

        public void Cleanup(Entity entity)
        {

        }
    }
}
