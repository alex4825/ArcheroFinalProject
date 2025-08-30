using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private bool _isRunning = false;

        private Entity _hero;
        private Entity _ghost;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = container.Resolve<EntitiesFactory>();
            _brainsFactory = container.Resolve<BrainsFactory>();
        }

        public void Run()
        {
            _hero = _entitiesFactory.CreateHero(Vector3.zero);
            _hero.AddCurrentTarget();
            _brainsFactory.CreateMainHeroBrain(_hero);

            _ghost = _entitiesFactory.CreateGhost(Vector3.zero + Vector3.forward * 5); 

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                _hero.TakeDamageRequest.Invoke(50);
                Debug.Log($"Текущий уровень здоровья: {_hero.CurrentHealth.Value.ToString()}");
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _hero.StartAttackRequest.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                _brainsFactory.CreateGhostBrain(_ghost);
            }
        }
    }
}
