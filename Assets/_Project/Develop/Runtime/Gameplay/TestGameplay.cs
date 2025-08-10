using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;

        private bool _isRunning = false;

        private Entity _entity;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            _entity = _entitiesFactory.CreateTestEntity(Vector3.zero);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            Debug.Log($"Направление движения: " + _entity.GetComponent<MoveDirection>().Value.Value.ToString());
            Debug.Log($"Скорость движения: " + _entity.GetComponent<MoveSpeed>().Value.Value.ToString());
        }
    }
}
