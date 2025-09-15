using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PlacementMinesState : State, IUpdatableState
    {
        public PlacementMinesState()
        {

        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Расстановка мин: ВХОД");
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Расстановка мин: ВЫХОД");

        }
    }
}