using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Cleanup
{
    public class OnEventContactsCleanSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private Buffer<Collider> _contacts;
        private IReadonlyEvent<Vector3> _detectedPointEvent;

        private IDisposable _detectedDisposable;

        private bool _isPointDetected;

        public OnEventContactsCleanSystem(IReadonlyEvent<Vector3> detectedPointEvent)
        {
            _detectedPointEvent = detectedPointEvent;
        }

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;

            _detectedDisposable = _detectedPointEvent.Subscribe(OnPointDetected);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isPointDetected)
            {
                _isPointDetected = false;
                _contacts.Clear();
            }
        }

        public void OnDispose(Entity entity)
        {
            _detectedDisposable.Dispose();
        }

        private void OnPointDetected(Vector3 vector) => _isPointDetected = true;
    }
}