using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class AttackEnemiesState : State, IUpdatableState
    {
        public AttackEnemiesState()
        {

        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Вход в состояние атаки врагов игроком");
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();

        }
    }
}