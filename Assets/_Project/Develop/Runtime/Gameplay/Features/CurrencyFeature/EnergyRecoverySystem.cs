using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature
{
    public class EnergyRecoverySystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        public ReactiveVariable<float> _initialEnergyCount;
        public ReactiveVariable<float> _currentEnergyCount;
        public ReactiveVariable<float> _recoveryEnergyCount;
        public ReactiveVariable<float> _timeToRecoverEnergy;

        private bool _inRecoveryProcess;
        private float _timerToRecover;

        private IDisposable _changedEnergyDisposable;

        public void OnInit(Entity entity)
        {
            _initialEnergyCount = entity.InitialEnergyCount;
            _currentEnergyCount = entity.CurrentEnergyCount;
            _recoveryEnergyCount = entity.RecoveryEnergyCount;
            _timeToRecoverEnergy = entity.TimeToRecoverEnergy;

            _timerToRecover = 0;

            _changedEnergyDisposable = _currentEnergyCount.Subscribe(OnChangedEnergy);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inRecoveryProcess)
                _timerToRecover += deltaTime;

            if (IsTimerExpire())
            {
                _timerToRecover = 0;
                _inRecoveryProcess = false;

                _currentEnergyCount.Value = MathF.Min(_currentEnergyCount.Value + _recoveryEnergyCount.Value, _initialEnergyCount.Value);
            }
        }

        public void OnDispose(Entity entity)
        {
            _changedEnergyDisposable.Dispose();
        }

        private void OnChangedEnergy(float arg1, float currentEnergy)
        {
            if (IsFullEnergy(currentEnergy))
            {
                Debug.Log("Полная энергия. Текущий уровень энергии: " + _currentEnergyCount.Value.ToString());
            }
            else
            {
                Debug.Log("Начался процесс восстановления энергии. Текущий уровень энергии: " + _currentEnergyCount.Value.ToString());
                _inRecoveryProcess = true;
            }
        }

        private bool IsFullEnergy(float currentEnergy)
            => _initialEnergyCount.Value == currentEnergy;

        private bool IsTimerExpire() => _timerToRecover >= _timeToRecoverEnergy.Value;
    }
}