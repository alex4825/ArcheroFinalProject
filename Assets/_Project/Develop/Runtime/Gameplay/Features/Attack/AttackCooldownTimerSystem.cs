using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class AttackCooldownTimerSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _inAttackCooldown;

        private ReactiveEvent _endAttackEvent;
        private ReactiveEvent _attackCooldownIsOverEvent;

        private IDisposable _endAttackEventDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.AttackCooldownCurrentTime;
            _initialTime = entity.AttackCooldownInitialTime;
            _inAttackCooldown = entity.InAttackCooldown;
            _endAttackEvent = entity.EndAttackEvent;
            _attackCooldownIsOverEvent = entity.AttackCooldownIsOverEvent;

            _endAttackEventDisposable = _endAttackEvent.Subscribe(OnEndAttack);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inAttackCooldown.Value == false)
                return;

            _currentTime.Value -= deltaTime;

            if (CooldownIsOver())
            {
                _inAttackCooldown.Value = false;
                _attackCooldownIsOverEvent.Invoke();
                Debug.Log("ÊÓËÄÀÓÍ ÇÀÊÎÍ×ÈËÑß");
            }
        }

        public void OnDispose(Entity entity)
        {
            _endAttackEventDisposable.Dispose();
        }

        private void OnEndAttack()
        {
            Debug.Log("ÊÓËÄÀÓÍ ÍÀ×ÀËÑß");
            _currentTime.Value = _initialTime.Value;
            _inAttackCooldown.Value = true;
        }

        private bool CooldownIsOver() => _currentTime.Value <= 0;
    }
}