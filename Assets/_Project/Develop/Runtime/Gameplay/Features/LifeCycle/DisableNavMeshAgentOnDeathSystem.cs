using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using System;
using UnityEngine.AI;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class DisableNavMeshAgentOnDeathSystem : IInitializableSystem, IDisposableSystem
    {
        private NavMeshAgent _agent;

        private IDisposable _isDeadChangedDisposable;

        public void OnInit(Entity entity)
        {
            _agent = entity.NavMeshAgent;

            _isDeadChangedDisposable = entity.IsDead.Subscribe(OnIsDeadChanged);
        }

        public void OnDispose(Entity entity)
        {
            _isDeadChangedDisposable.Dispose();
        }

        private void OnIsDeadChanged(bool arg1, bool isDead)
        {
            if (isDead)
                _agent.isStopped = true;
        }
    }
}
