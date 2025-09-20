using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Environment;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.TargetSelection;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly IInputService _inputService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
            _timerServiceFactory = container.Resolve<TimerServiceFactory>();
            _brainsContext = container.Resolve<AIBrainsContext>();
            _inputService = container.Resolve<IInputService>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateMainHeroSelfControlShootBrain(Entity entity)
        {
            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            AIStateMachine combatState = CreateInputRotateAttackStateMachine(entity);

            ICompositeCondition fromMovementToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => _inputService.MoveDirection == Vector3.zero));

            ICompositeCondition fromCombatToMovementStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => _inputService.MoveDirection != Vector3.zero));

            AIStateMachine behavior = new AIStateMachine();

            behavior.AddState(combatState);
            behavior.AddState(movementState);

            behavior.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            behavior.AddTransition(combatState, movementState, fromCombatToMovementStateCondition);

            StateMachineBrain brain = new StateMachineBrain(behavior);
            _brainsContext.SetFor(entity, brain);

            return brain;

        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine combatState = CreateAutoAttackStateMachine(entity);

            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromMovementToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget != null))
                .Add(new FuncCondition(() => _inputService.MoveDirection == Vector3.zero));

            ICompositeCondition fromCombatToMovementStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => currentTarget == null))
                .Add(new FuncCondition(() => _inputService.MoveDirection != Vector3.zero));

            AIStateMachine behavior = new AIStateMachine();

            behavior.AddState(combatState);
            behavior.AddState(movementState);

            behavior.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            behavior.AddTransition(combatState, movementState, fromCombatToMovementStateCondition);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behavior);

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;

        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateRandomMovementStateMashine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateExplodyBrain(Entity explody)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            Entity fortress = _container.Resolve<FortressHolderService>().Fortress;
            explody.CurrentTarget.Value = fortress;

            NavMeshMoveToTargetState moveToFortressState = new NavMeshMoveToTargetState(explody);

            DeathState deathState = new DeathState(explody);

            Exploder exploder = new(
                _container.Resolve<CollidersRegistryService>(),
                explody.ExplodeRadius,
                explody.ExplodeDamage,
                Layers.EntityMask,
                explody.Team);

            disposables.Add(deathState.Entered.Subscribe(
                () => exploder.ExplodeIn(explody.Transform.position)));

            AIStateMachine rootStateMachine = new AIStateMachine(disposables);
            rootStateMachine.AddState(moveToFortressState);
            rootStateMachine.AddState(deathState);

            rootStateMachine.AddTransition(moveToFortressState, deathState, new CompositeCondition()
                .Add(new FuncCondition(() => fortress.IsDead.Value == false))
                .Add(new FuncCondition(() => explody.IsDead.Value == false))
                .Add(new FuncCondition(() => Vector3.Distance(fortress.Transform.position, explody.Transform.position) <= explody.ExplodeRadius.Value)));

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(explody, brain);

            return brain;
        }

        public StateMachineBrain CreateMineBrain(Entity mine)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            FindTargetState findTargetState = new FindTargetState(new NearestDamageableTargetSelector(mine), _entitiesLifeContext, mine);

            DeathState deathState = new DeathState(mine);

            Exploder exploder = new(
                _container.Resolve<CollidersRegistryService>(),
                mine.ExplodeRadius,
                mine.ExplodeDamage,
                Layers.EntityMask,
                mine.Team);

            disposables.Add(deathState.Entered.Subscribe(
                () => exploder.ExplodeIn(mine.Transform.position)));

            AIStateMachine rootStateMachine = new AIStateMachine();
            rootStateMachine.AddState(findTargetState);
            rootStateMachine.AddState(deathState);

            rootStateMachine.AddTransition(findTargetState, deathState, new CompositeCondition()
                .Add(new FuncCondition(() => mine.IsDead.Value == false))
                .Add(new FuncCondition(() =>
                    mine.CurrentTarget.Value != null
                    && Vector3.Distance(mine.CurrentTarget.Value.Transform.position, mine.Transform.position) <= mine.ExplodeRadius.Value)));

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(mine, brain);

            return brain;
        }

        public StateMachineBrain CreateMinatoBrain(Entity minato, ITargetSelector targetSelector)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, minato);

            RandomTeleportState randomTeleportState = new RandomTeleportState(minato);
            TargetOrientedTeleportState targetOrientedTeleportState = new TargetOrientedTeleportState(minato, targetSelector, _entitiesLifeContext);

            Entity targetEntity = minato.CurrentTarget.Value;

            AIStateMachine rootStateMachine = new AIStateMachine();

            rootStateMachine.AddState(targetOrientedTeleportState);
            rootStateMachine.AddState(randomTeleportState);

            rootStateMachine.AddTransition(targetOrientedTeleportState, randomTeleportState, new FuncCondition(() => targetEntity == null));
            rootStateMachine.AddTransition(randomTeleportState, targetOrientedTeleportState, new FuncCondition(() => targetEntity != null));

            Exploder exploder = new(
                _container.Resolve<CollidersRegistryService>(),
                minato.ExplodeRadius,
                minato.ExplodeDamage,
                Layers.EntityMask,
                minato.Team);

            minato.TeleportedEvent.Subscribe(exploder.ExplodeIn);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);

            _brainsContext.SetFor(minato, brain);

            return brain;
        }

        private AIStateMachine CreateRandomMovementStateMashine(Entity entity)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            RandomMovementState randomMovementState = new RandomMovementState(entity, 0.5f);

            EmptyState emptyState = new EmptyState();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new FuncCondition(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new FuncCondition(() => idleTimer.IsOver);

            AIStateMachine stateMachine = new AIStateMachine(disposables);

            stateMachine.AddState(randomMovementState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            stateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return stateMachine;
        }

        private AIStateMachine CreateInputRotateAttackStateMachine(Entity entity)
        {
            PlayerInputRotationState rotationState = new PlayerInputRotationState(entity, _inputService);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            List<IDisposable> disposables = new();

            bool needAttack = false;

            disposables.Add(_inputService.Attacked.Subscribe(() => needAttack = true));

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(entity.CanStartAttack)
                .Add(new FuncCondition(() =>
                {
                    bool needAttackCashed = needAttack;
                    needAttack = false;
                    return needAttackCashed;
                }));

            ICondition fromAttackToRotateStateCondition = new FuncCondition(() => entity.InAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine(disposables);

            stateMachine.AddState(rotationState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotationState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotationState, fromAttackToRotateStateCondition);

            return stateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));

                    return angleToTarget < 3f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;
            ICondition fromAttackToRotateStateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);

            return stateMachine;
        }
    }
}