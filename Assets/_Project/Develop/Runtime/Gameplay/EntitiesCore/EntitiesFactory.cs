using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
        }

        public Entity CreateGhost(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Ghost");

            entity.AddMoveDirection()
                  .AddMoveSpeed(new ReactiveVariable<float>(10))
                  .AddRotationDirection()
                  .AddRotationSpeed(new ReactiveVariable<float>(500))
                  .AddMaxHealth(new ReactiveVariable<float>(100))
                  .AddCurrentHealth(new ReactiveVariable<float>(100))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                  .AddDeathProcessCurrentTime();

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
