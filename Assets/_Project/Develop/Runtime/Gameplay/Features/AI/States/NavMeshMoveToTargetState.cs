using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class NavMeshMoveToTargetState : State, IUpdatableState
    {
        private NavMeshAgent _agent;
        private Transform _target;
        private ReactiveVariable<bool> _isMoving;

        public NavMeshMoveToTargetState(Entity entity)
        {
            _target = entity.CurrentTarget.Value.Transform;
            _agent = entity.NavMeshAgent;
            _isMoving = entity.IsMoving;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.SetDestination(_target.position);
            _isMoving.Value = true;
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();

            _agent.isStopped = true;
            _isMoving.Value = false;
        }
    }
}