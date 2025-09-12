using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayParallelState : ParallelState<IUpdatableState>, IUpdatableState
    {
        public GameplayParallelState(params IUpdatableState[] states) : base(states)
        {

        }

        public void Update(float deltaTime)
        {
            foreach (IUpdatableState state in States)
                state.Update(deltaTime);
        }
    }
}