using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Projectiles;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public class ProjectilesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly CollidersRegistryService _collidersRegistryService;
        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        public ProjectilesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
            _collidersRegistryService = container.Resolve<CollidersRegistryService>();
        }

        public Entity CreateArrow(ArrowConfig config, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, owner.ShootPoint.position, config.PrefabPath);

            entity.AddMoveDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                  .AddIsMoving()
                  .AddRotationDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                  .AddIsDead()
                  .AddContactsDetectingMask(Layers.EntityMask | Layers.EnvironmentMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddBodyContactDamage(new ReactiveVariable<float>(config.Damage))
                  .AddDeathMask(Layers.EnvironmentMask)
                  .AddIsTouchDeathMask()
                  .AddIsTouchAnotherTeam()
                  .AddTeam(new ReactiveVariable<Teams>(owner.Team.Value));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DealDamageOnContactSystem(entity.BodyContactDamage))
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new AnotherTeamTouchDetectorSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity Create—annonball(CannonballConfig config, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, owner.ShootPoint.position, config.PrefabPath);

            entity.AddMoveDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                  .AddIsMoving()
                  .AddRotationDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                  .AddIsDead()
                  .AddContactsDetectingMask(Layers.EntityMask | Layers.EnvironmentMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddExplodeDamage(new ReactiveVariable<float>(config.ExplodeDamage))
                  .AddExplodeRadius(new ReactiveVariable<float>(config.ExplodeRadius))
                  .AddExplodedEvent()
                  .AddDeathMask(Layers.EnvironmentMask)
                  .AddIsTouchDeathMask()
                  .AddIsTouchAnotherTeam()
                  .AddTeam(new ReactiveVariable<Teams>(owner.Team.Value));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustExplode = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustExplode(mustExplode)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new AnotherTeamTouchDetectorSystem())
                  .AddSystem(new ExplodeSystem(_collidersRegistryService))
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateMissile(MissileConfig config, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, owner.ShootPoint.position, config.PrefabPath);

            entity.AddMoveDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                  .AddIsMoving()
                  .AddRotationDirection(new ReactiveVariable<Vector3>(owner.ShootPoint.forward))
                  .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                  .AddIsDead()
                  .AddContactsDetectingMask(Layers.EntityMask | Layers.EnvironmentMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddExplodeDamage(new ReactiveVariable<float>(config.ExplodeDamage))
                  .AddExplodeRadius(new ReactiveVariable<float>(config.ExplodeRadius))
                  .AddExplodedEvent()
                  .AddDeathMask(Layers.EnvironmentMask)
                  .AddIsTouchDeathMask()
                  .AddIsTouchAnotherTeam()
                  .AddTeam(new ReactiveVariable<Teams>(owner.Team.Value));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustExplode = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value))
                .Add(new FuncCondition(() => entity.IsTouchAnotherTeam.Value));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustExplode(mustExplode)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new AnotherTeamTouchDetectorSystem())
                  .AddSystem(new ExplodeSystem(_collidersRegistryService))
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }


        private Entity CreateEmpty() => new Entity();
    }
}