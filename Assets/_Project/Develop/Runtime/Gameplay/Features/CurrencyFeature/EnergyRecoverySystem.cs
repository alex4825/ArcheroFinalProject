using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature
{
    public class EnergyRecoverySystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _initialEnergyCount;
        private ReactiveVariable<float> _currentEnergyCount;
        private ReactiveVariable<float> _timeToRecoverEnergy;
        private ReactiveVariable<float> _recoveryEnergyCountKoef;

        private ReactiveEvent<float> _addEnergyRequest;
        private ReactiveEvent _fullEnergyEvent;

        private float _recoveryEnergyCount;
        private bool _inRecoveryProcess;
        private float _timerToRecover;

        private IDisposable _currentEnergyDisposable;
        private IDisposable _fullEnergyDisposable;

        public void OnInit(Entity entity)
        {
            _initialEnergyCount = entity.InitialEnergyCount;
            _currentEnergyCount = entity.CurrentEnergyCount;
            _timeToRecoverEnergy = entity.TimeToRecoverEnergy;
            _recoveryEnergyCountKoef = entity.RecoveryEnergyCountKoef;
            _addEnergyRequest = entity.AddEnergyCountRequest;
            _fullEnergyEvent = entity.FullEnergyEvent;

            _recoveryEnergyCount = _initialEnergyCount.Value * _recoveryEnergyCountKoef.Value;

            _currentEnergyDisposable = _currentEnergyCount.Subscribe(OnEnergyCountChanged);
            _fullEnergyDisposable = _fullEnergyEvent.Subscribe(OnFullEnergy);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inRecoveryProcess)
                _timerToRecover += deltaTime;

            if (IsTimerExpire())
            {
                _timerToRecover = 0;

                _addEnergyRequest.Invoke(_recoveryEnergyCount);
            }
        }

        public void OnDispose(Entity entity)
        {
            _currentEnergyDisposable.Dispose();
            _fullEnergyDisposable.Dispose();
        }

        private void OnEnergyCountChanged(float arg1, float arg2) => _inRecoveryProcess = true;

        private void OnFullEnergy() => _inRecoveryProcess = false;

        private bool IsTimerExpire() => _timerToRecover >= _timeToRecoverEnergy.Value;
    }
}