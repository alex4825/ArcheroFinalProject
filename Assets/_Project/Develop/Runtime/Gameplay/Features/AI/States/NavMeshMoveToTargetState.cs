using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;
using UnityEngine.AI;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class NavMeshMoveToTargetState : State, IUpdatableState
    {
        private NavMeshAgent _agent;
        private Transform _target;

        public NavMeshMoveToTargetState(Entity entity)
        {
            _target = entity.CurrentTarget.Value.Transform;
            _agent = entity.NavMeshAgent;
        }

        public override void Enter()
        {
            base.Enter();

            _agent.SetDestination(_target.position);
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();

            _agent.isStopped = true;
        }
    }
}