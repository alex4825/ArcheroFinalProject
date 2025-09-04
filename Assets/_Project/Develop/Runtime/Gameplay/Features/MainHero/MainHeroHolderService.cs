using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroHolderService : IInitializable, IDisposable
    {
        private EntitiesLifeContext _entitiesLifeContext;
        private ReactiveEvent<Entity> _heroRegistered = new();

        private Entity _mainHero;

        public MainHeroHolderService(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public IReadonlyEvent<Entity> HeroRegistered => _heroRegistered;

        public Entity MainHero => _mainHero;

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
        }

        private void OnEntityAdded(Entity entity)
        {
            if (entity.HasComponent<IsMainHero>())
            {
                _entitiesLifeContext.Added -= OnEntityAdded;
                _mainHero = entity;
                _heroRegistered?.Invoke(entity);
            }
        }
    }
}