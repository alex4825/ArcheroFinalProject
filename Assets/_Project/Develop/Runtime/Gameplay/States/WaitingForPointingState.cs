using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WaitingForPointingState : State, IUpdatableState
    {
        private IInputService _inputService;

        private ReactiveEvent<Vector3> _pointFound = new();

        private IDisposable _pointedDisposable;

        public WaitingForPointingState(IInputService inputService)
        {
            _inputService = inputService;
        }

        public IReadonlyEvent<Vector3> PointFound => _pointFound;

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Процесс атаки врагов: ВХОД");

            _pointedDisposable = _inputService.Pointed.Subscribe(OnPointed);
        }

        public void Update(float deltaTime)
        {

        }

        public override void Exit()
        {
            base.Exit();
            Debug.Log("Процесс атаки врагов: ВЫХОД");

            _pointedDisposable.Dispose();
        }

        private void OnPointed(Vector3 position)
        {
            _pointFound.Invoke(position);
        }
    }
}