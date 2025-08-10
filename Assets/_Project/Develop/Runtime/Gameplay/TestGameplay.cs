using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;

        private bool _isRunning = false;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            Entity entity = _entitiesFactory.CreqateTestEntity();

            Debug.Log($"Направление движения: " + entity.GetComponent<MoveDirection>().Value.Value.ToString());
            Debug.Log($"Скорость движения: " + entity.GetComponent<MoveSpeed>().Value.Value.ToString());

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;
        }
    }
}
