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
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

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

        public Entity CreateHero(Vector3 position, HeroConfig config)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Hero");

            entity.AddMoveDirection()
                  .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                  .AddIsMoving()
                  .AddRotationDirection()
                  .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                  .AddMaxHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddCurrentHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(config.DeathProcessTime))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddAttackProcessInitialTime(new ReactiveVariable<float>(config.AttackProcessTime))
                  .AddAttackProcessCurrentTime()
                  .AddInAttackProcess()
                  .AddStartAttackRequest()
                  .AddStartAttackEvent()
                  .AddEndAttackEvent()
                  .AddAttackDelayTime(new ReactiveVariable<float>(config.AttackDelayTime))
                  .AddAttackDelayEndEvent()
                  .AddInstantAttackDamage(new ReactiveVariable<float>(config.InstantAttackDamage))
                  .AddAttackCancelEvent()
                  .AddAttackCooldownInitialTime(new ReactiveVariable<float>(config.AttackCooldown))
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

            return entity;
        }

        public Entity CreateGhost(Vector3 position, GhostConfig config, Teams team)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Ghost");

            entity.AddTeam(new ReactiveVariable<Teams>(team))
                  .AddMoveDirection()
                  .AddMoveSpeed(new ReactiveVariable<float>(config.MoveSpeed))
                  .AddIsMoving()
                  .AddRotationDirection()
                  .AddRotationSpeed(new ReactiveVariable<float>(config.RotationSpeed))
                  .AddMaxHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddCurrentHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(config.DeathProcessTime))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddContactsDetectingMask(Layers.EntityMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddBodyContactDamage(new ReactiveVariable<float>(config.BodyContactDamage));

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
                  .AddSystem(new DealDamageOnContactSystem(entity.BodyContactDamage))
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateMinato(Vector3 position, Teams team)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Minato");

            entity.AddTeam(new ReactiveVariable<Teams>(team))
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
                  .AddInitialEnergyCount(new ReactiveVariable<float>(300))
                  .AddCurrentEnergyCount(new ReactiveVariable<float>(300))
                  .AddRecoveryEnergyCountKoef(new ReactiveVariable<float>(0.1f))
                  .AddTimeToRecoverEnergy(new ReactiveVariable<float>(1f))
                  .AddAddEnergyCountRequest()
                  .AddSubtractEnergyCountRequest()
                  .AddFullEnergyEvent()
                  .AddTeleportEnergyCost(new ReactiveVariable<float>(120))
                  .AddTeleportMaxRadius(new ReactiveVariable<float>(3))
                  .AddTeleportedEvent()
                  .AddTeleportDelay(new ReactiveVariable<float>(1f))
                  .AddExplodeRadius(new ReactiveVariable<float>(4))
                  .AddExplodeDamage(new ReactiveVariable<float>(60))
                  .AddDeathMask(1 << LayerMask.NameToLayer("Characters"))
                  .AddIsTouchDeathMask()
                  .AddCurrentTarget();

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeadProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            float minEnergyKoef = 0.4f;

            ICompositeCondition canTeleport = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false))
                .Add(new FuncCondition(() => entity.CurrentEnergyCount.Value >= entity.TeleportEnergyCost.Value))
                .Add(new FuncCondition(() => entity.CurrentEnergyCount.Value >= entity.InitialEnergyCount.Value * minEnergyKoef));

            entity
                .AddCanMove(canMove)
                .AddMustDie(mustDie)
                .AddMustSelfRelease(mustSelfRelease)
                .AddCanApplyDamage(canApplyDamage)
                .AddCanTeleport(canTeleport);

            entity.AddSystem(new EnergyRegulateSystem())
                  .AddSystem(new EnergyRecoverySystem())
                  .AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathMaskTouchDetectorSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateProjectile(Vector3 position, Vector3 direction, float damage, Entity owner)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/Projectile");

            entity.AddMoveDirection(new ReactiveVariable<Vector3>(direction))
                  .AddMoveSpeed(new ReactiveVariable<float>(10))
                  .AddIsMoving()
                  .AddRotationDirection(new ReactiveVariable<Vector3>(direction))
                  .AddRotationSpeed(new ReactiveVariable<float>(9999))
                  .AddIsDead()
                  .AddContactsDetectingMask(Layers.EntityMask | Layers.EnvironmentMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddBodyContactDamage(new ReactiveVariable<float>(damage))
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

        public Entity CreateExplody(Vector3 position, ExplodyConfig config, Teams team)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, config.PrefabPath);

            entity.AddTeam(new ReactiveVariable<Teams>(team))
                  .AddMaxHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddCurrentHealth(new ReactiveVariable<float>(config.MaxHealth))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(config.DeathProcessTime))
                  .AddDeathProcessCurrentTime()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddContactsDetectingMask(Layers.EntityMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                  .AddExplodeRadius(new ReactiveVariable<float>(config.ExplodeRadius))
                  .AddExplodeDamage(new ReactiveVariable<float>(config.ExplodeDamage))
                  .AddExplodedEvent()
                  .AddCurrentTarget()
                  .AddDisableCollidersOnDeath();

            entity.NavMeshAgent.speed = config.MoveSpeed;
            entity.NavMeshAgent.angularSpeed = config.RotationSpeed;

            ICompositeCondition canMove = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition mustSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value))
                .Add(new FuncCondition(() => entity.InDeadProcess.Value == false));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            entity.AddCanMove(canMove)
                   .AddMustDie(mustDie)
                   .AddMustSelfRelease(mustSelfRelease)
                   .AddCanApplyDamage(canApplyDamage);

            entity.AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DisableNavMeshAgentOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem())
                  .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateFortress(MonoEntity fortress)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.AddExisting(entity, fortress);

            entity.AddIsFortress()
                  .AddMaxHealth(new ReactiveVariable<float>(400))
                  .AddCurrentHealth(new ReactiveVariable<float>(400))
                  .AddIsDead()
                  .AddInDeadProcess()
                  .AddDeathProcessInitialTime(new ReactiveVariable<float>(2))
                  .AddDeathProcessCurrentTime()
                  .AddDisableCollidersOnDeath()
                  .AddTakeDamageRequest()
                  .AddTakeDamageEvent()
                  .AddTeam(new ReactiveVariable<Teams>(Teams.MainHero));

            ICompositeCondition mustDie = new CompositeCondition()
                .Add(new FuncCondition(() => entity.CurrentHealth.Value <= 0));

            ICompositeCondition canApplyDamage = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            entity
                .AddMustDie(mustDie)
                .AddCanApplyDamage(canApplyDamage);

            entity.AddSystem(new ApplyDamageSystem())
                  .AddSystem(new DeathSystem())
                  .AddSystem(new DisableCollidersOnDeathSystem())
                  .AddSystem(new DeathProcessTimerSystem());

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateMine(Vector3 position, MineConfig config, Teams team)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, config.PrefabPath);

            entity
                .AddTeam(new ReactiveVariable<Teams>(team))
                .AddIsDead()
                .AddExplodedEvent()
                .AddCurrentTarget()
                .AddDisableCollidersOnDeath()
                .AddContactsDetectingMask(Layers.EntityMask)
                .AddContactCollidersBuffer(new Buffer<Collider>(64))
                .AddContactEntitiesBuffer(new Buffer<Entity>(64))
                .AddExplodeDamage(new ReactiveVariable<float>(config.ExplodeDamage))
                .AddExplodeRadius(new ReactiveVariable<float>(config.ExplodeRadius));

            ICompositeCondition musSelfRelease = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value));

            entity
                .AddMustSelfRelease(musSelfRelease);

            entity
                .AddSystem(new DisableCollidersOnDeathSystem())
                .AddSystem(new SelfReleaseSystem(_entitiesLifeContext));

            return entity;
        }

        public Entity CreateContactTrigger(Vector3 position)
        {
            Entity entity = CreateEmpty();

            _monoEntitiesFactory.Create(entity, position, "Entities/ContactTrigger");

            entity.AddContactsDetectingMask(Layers.EntityMask)
                  .AddContactCollidersBuffer(new Buffer<Collider>(64))
                  .AddContactEntitiesBuffer(new Buffer<Entity>(64));

            entity.AddSystem(new BodyContactDetectingSystem())
                  .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateEmpty() => new Entity();
    }
}
