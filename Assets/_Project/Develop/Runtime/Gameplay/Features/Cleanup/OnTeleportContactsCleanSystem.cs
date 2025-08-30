using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities;
using System;
using UnityEngine;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Cleanup
{
    public class OnTeleportContactsCleanSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private Buffer<Collider> _contacts;
        private ReactiveEvent<Vector3> _teleportedEvent;

        private IDisposable _teleportedDisposable;

        private bool _isTeleported;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _teleportedEvent = entity.TeleportedEvent;

            _teleportedDisposable = _teleportedEvent.Subscribe(OnTeleported);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isTeleported)
            {
                _isTeleported = false;
                _contacts.Clear();
            }
        }

        public void OnDispose(Entity entity)
        {
            _teleportedDisposable.Dispose();
        }

        private void OnTeleported(Vector3 vector) => _isTeleported = true;
    }
}