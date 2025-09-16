using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class DeathState : State, IUpdatableState
    {
        private ReactiveEvent<float> _takeDamageRequest;
        private ReactiveVariable<float> _maxHealth;

        public DeathState(Entity entity)
        {
            _takeDamageRequest = entity.TakeDamageRequest;
            _maxHealth = entity.MaxHealth;
        }

        public override void Enter()
        {
            base.Enter();

            _takeDamageRequest.Invoke(_maxHealth.Value);
        }

        public void Update(float deltaTime)
        {

        }
    }
}