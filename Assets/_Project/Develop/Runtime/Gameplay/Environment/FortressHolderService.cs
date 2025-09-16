using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Environment
{
    public class FortressHolderService : IInitializable, IDisposable
    {
        private EntitiesLifeContext _entitiesLifeContext;

        private Entity _fortress;

        private IDisposable _fortressDisposable;

        public FortressHolderService(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public Entity Fortress => _fortress;

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
            _fortressDisposable?.Dispose();
        }

        private void OnEntityAdded(Entity entity)
        {
            if (entity.HasComponent<IsFortress>())
            {
                _entitiesLifeContext.Added -= OnEntityAdded;
                _fortress = entity;
                _fortressDisposable = _fortress.CurrentHealth.Subscribe(OnFortressHPChanged);
                Debug.Log("Fortress on the scene");
            }
        }

        private void OnFortressHPChanged(float arg1, float currentHP)
            => Debug.Log($"Крепость получила урон. Текущий HP: {currentHP}");
    }
}