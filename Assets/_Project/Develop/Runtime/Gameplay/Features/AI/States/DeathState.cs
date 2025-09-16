using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class DeathState : State, IUpdatableState
    {
        private ReactiveVariable<bool> _isDead;

        public DeathState(Entity entity)
        {
            _isDead = entity.IsDead;
        }

        public override void Enter()
        {
            base.Enter();

            _isDead.Value = true;
        }

        public void Update(float deltaTime)
        {

        }
    }
}