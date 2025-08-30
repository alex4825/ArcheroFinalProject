using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class RadiusContactsOnTeleportDetectingSystem : IInitializableSystem, IDisposableSystem
    {
        private Buffer<Collider> _contacts;
        private CapsuleCollider _body;
        private LayerMask _mask;
        private ReactiveVariable<float> _radius;
        private ReactiveEvent<Vector3> _teleportedEvent;

        private IDisposable _teleportedDisposable;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _body = entity.BodyCollider;
            _mask = entity.ContactsDetectingMask;
            _radius = entity.OnTeleportExplodeRadius;
            _teleportedEvent = entity.TeleportedEvent;

            _teleportedDisposable = _teleportedEvent.Subscribe(OnTeleported);
        }

        public void OnDispose(Entity entity)
        {
            _teleportedDisposable.Dispose();
        }

        private void OnTeleported(Vector3 vector)
        {
            DetectContacts();
        }

        private void DetectContacts()
        {
            _contacts.Count = Physics.OverlapSphereNonAlloc(
                 _body.transform.position,
                 _radius.Value,
                 _contacts.Items,
                 _mask,
                 QueryTriggerInteraction.Ignore);

            RemoveSelfFromContacts();
        }

        private void RemoveSelfFromContacts()
        {
            int indexToRemove = -1;

            for (int i = 0; i < _contacts.Count; i++)
            {
                if (_contacts.Items[i] == _body)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove >= 0)
            {
                for (int i = indexToRemove; i < _contacts.Count - 1; i++)
                {
                    _contacts.Items[i] = _contacts.Items[i + 1];
                }

                _contacts.Count--;
            }
        }
    }
}