using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono
{
    public class MonoEntity : MonoBehaviour
    {
        private CollidersRegistryService _collidersRegistryService;

        private Entity _linkedEntity;

        public Entity LinkedEntity => _linkedEntity;

        public void Initialize(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void Link(Entity entity)
        {
            _linkedEntity = entity;

            MonoEntityRegistrator[] monoEntityRegistrators = GetComponentsInChildren<MonoEntityRegistrator>();

            if (monoEntityRegistrators != null)
                foreach (var registrator in monoEntityRegistrators)
                    registrator.Register(entity);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _collidersRegistryService.Register(collider, entity);
        }

        public void Cleanup(Entity entity)
        {
            _linkedEntity = null;

            foreach (Collider collider in GetComponentsInChildren<Collider>())
                _collidersRegistryService.Unregister(collider);
        }
    }
}
