using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Drawing;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Sensors
{
    public class RadiusContactsDetectingSystem : IInitializableSystem, IUpdatableSystem
    {
        private Buffer<Collider> _contacts;
        private CapsuleCollider _body;
        private LayerMask _mask;
        private ReactiveVariable<float> _radius;

        public void OnInit(Entity entity)
        {
            _contacts = entity.ContactCollidersBuffer;
            _body = entity.BodyCollider;
            _mask = entity.ContactsDetectingMask;
            _radius = entity.ExplodeRadius;
        }

        public void OnUpdate(float deltaTime)
        {
            DetectContactsIn(_body.transform.position);
        }

        private void DetectContactsIn(Vector3 point)
        {
            _contacts.Count = Physics.OverlapSphereNonAlloc(
                 point,
                 _radius.Value,
                 _contacts.Items,
                 _mask,
                 QueryTriggerInteraction.Ignore);

            if (_body != null)
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