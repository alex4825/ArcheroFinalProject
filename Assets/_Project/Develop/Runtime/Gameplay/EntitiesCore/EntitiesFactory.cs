using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot;
using Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature.TeleportMovement;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private readonly CollidersRegistryService _collidersRegistryService;
        private readonly MonoEntitiesFactory _monoEntitiesFactory;

        public EntitiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
            _monoEntitiesFactory = container.Resolve<MonoEntitiesFactory>();
            _collidersRegistryService = container.Resolve<CollidersRegistryService>();
        }

        public Entity CreateHero(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Hero");

            entity.AddMoveDirection()
                  .AddMoveSpeed(new ReactiveVariable<float>(10))
                  .AddIsMoving()
                  .AddRotationDirection()
                  .AddRotationSpeed(new ReactiveVariable<float>(500))
                  .AddMaxHealth(new ReactiveVariable<float>(100))
                  .AddCurrentHealth(new ReactiveVariable<float>(100))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddAttackProcessInitialTime(new ReactiveVariable<float>(3))
                  .AddAttackProcessCurrentTime()
                  .AddInAttackProcess()
                  .AddStartAttackRequest()
                  .AddStartAttackEvent()
                  .AddEndAttackEvent()
                  .AddAttackDelayTime(new ReactiveVariable<float>(1))
                  .AddAttackDelayEndEvent()
                  .AddInstantAttackDamage(new ReactiveVariable<float>(50))
                  .AddAttackCancelEvent()
                  .AddAttackCooldownInitialTime(new ReactiveVariable<float>(2))
                  .AddAttackCooldownCurrentTime()
                  .AddInAttackCooldown();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeadProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canStartAttack = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.InAttackProcess.Value == false))
                .Add(new FuncCondition(() => entity.IsMoving.Value == false))
                .Add(new FuncCondition(() => entity.InAttackCooldown.Value == false));

            ICompositeCondition mustCancelAttack = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.IsMoving.Value));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddCanStartAttack(canStartAttack)
                .AddMustCalcelAttack(mustCancelAttack);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new AttackCancelSystem())
                  .AddSystem(new StartAttackSystem())
                  .AddSystem(new AttackProcessTimerSystem())
                  .AddSystem(new AttackDelayEndTriggerSystem())
                  .AddSystem(new InstantShootSystem(this))
                  .AddSystem(new EndAttackSystem())
                  .AddSystem(new AttackCooldownTimerSystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateGhost(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Ghost");

            entity.AddMoveDirection()
                  .AddMoveSpeed(new ReactiveVariable<float>(10))
                  .AddIsMoving()
                  .AddRotationDirection()
                  .AddRotationSpeed(new ReactiveVariable<float>(500))
                  .AddMaxHealth(new ReactiveVariable<float>(100))
                  .AddCurrentHealth(new ReactiveVariable<float>(100))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddContactsDetectingMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddBodyContactDamage(new ReactiveVariable<float>(50));

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeadProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            entity
                .AddCanMove(canMove)
                .AddCanRotate(canRotate)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage);

            entity.AddSystem(new RigidbodyMovementSystem())
                  .AddSystem(new RigidbodyRotationSystem())
                  .AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DealDamageOnContactSystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateMinato(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Minato");

            entity.AddMaxHealth(new ReactiveVariable<float>(100))
                  .AddCurrentHealth(new ReactiveVariable<float>(100))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddContactsDetectingMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddInitialEnergyCount(new ReactiveVariable<float>(150))
                  .AddCurrentEnergyCount(new ReactiveVariable<float>(150))
                  .AddRecoveryEnergyCountKoef(new ReactiveVariable<float>(0.1f))
                  .AddTimeToRecoverEnergy(new ReactiveVariable<float>(0.3f))
                  .AddAddEnergyCountRequest()
                  .AddSubtractEnergyCountRequest()
                  .AddFullEnergyEvent()
                  .AddTeleportEnergyCost(new ReactiveVariable<float>(100))
                  .AddTeleportMaxRadius(new ReactiveVariable<float>(5))
                  .AddTeleportedEvent()
                  .AddOnTeleportExplodeRadius(new ReactiveVariable<float>(3))
                  .AddBodyContactDamage(new ReactiveVariable<float>(60))
                  .AddDeathMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddIsTouchDeathMask();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeadProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canTeleport = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.CurrentEnergyCount.Value >= entity.TeleportEnergyCost.Value));

            entity
                .AddCanMove(canMove)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddCanTeleport(canTeleport);

            entity.AddSystem(new EnergyRegulateSystem())
                  .AddSystem(new EnergyRecoverySystem())
                  .AddSystem(new RandomTeleportMovementSystem())
                  .AddSystem(new RadiusContactsOnTeleportDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService))
                  .AddSystem(new DealDamageOnContactSystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateProjectile(Vector3 position, Vector3 direction, float damage)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Projectile");

            entity.AddMoveDirection(new ReactiveVariable<Vector3>(direction))
                  .AddMoveSpeed(new ReactiveVariable<float>(10))
                  .AddIsMoving()
                  .AddRotationDirection(new ReactiveVariable<Vector3>(direction))
                  .AddRotationSpeed(new ReactiveVariable<float>(9999))
                  .AddIsDead()
                  .AddContactsDetectingMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddBodyContactDamage(new ReactiveVariable<float>(damage))
                  .AddDeathMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddIsTouchDeathMask();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition canRotate = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsTouchDeathMask.Value));

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
                  .AddSystem(new DealDamageOnContactSystem())
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
