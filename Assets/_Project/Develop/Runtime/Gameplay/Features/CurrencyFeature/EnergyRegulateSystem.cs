using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.CurrencyFeature
{
    public class EnergyRegulateSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _initialEnergyCount;
        private ReactiveVariable<float> _currentEnergyCount;
        private ReactiveEvent<float> _addEnergyCountRequest;
        private ReactiveEvent<float> _subtractEnergyCountRequest;

        private ReactiveEvent _fullEnergyEvent;

        private IDisposable _addEnergyCountDisposable;
        private IDisposable _subtractEnergyCountDisposable;

        public void OnInit(Entity entity)
        {
            _initialEnergyCount = entity.InitialEnergyCount;
            _currentEnergyCount = entity.CurrentEnergyCount;
            _addEnergyCountRequest = entity.AddEnergyCountRequest;
            _subtractEnergyCountRequest = entity.SubtractEnergyCountRequest;

            _fullEnergyEvent = entity.FullEnergyEvent;

            _addEnergyCountDisposable = _addEnergyCountRequest.Subscribe(OnAddEnergyCount);
            _subtractEnergyCountDisposable = _subtractEnergyCountRequest.Subscribe(OnSubtractEnergyCount);
        }

        public void OnDispose(Entity entity)
        {
            _addEnergyCountDisposable.Dispose();
            _subtractEnergyCountDisposable.Dispose();
        }

        private void OnAddEnergyCount(float amount)
        {
            _currentEnergyCount.Value = MathF.Min(_currentEnergyCount.Value + amount, _initialEnergyCount.Value);

            Debug.Log("Текущий уровень энергии: " + _currentEnergyCount.Value.ToString());

            if (_currentEnergyCount.Value == _initialEnergyCount.Value)
                _fullEnergyEvent?.Invoke();
        }

        private void OnSubtractEnergyCount(float amount)
        {
            _currentEnergyCount.Value = MathF.Max(_currentEnergyCount.Value - amount, 0);
            Debug.Log("Текущий уровень энергии: " + _currentEnergyCount.Value.ToString());
        }
    }
}