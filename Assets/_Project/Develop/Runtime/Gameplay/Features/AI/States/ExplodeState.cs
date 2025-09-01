using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class ExplodeState : State, IUpdatableState
    {
        private Buffer<Collider> _contactsColliders;
        private Buffer<Entity> _contactsEntities;

        private CapsuleCollider _body;
        private LayerMask _mask;
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<float> _damage;
        private ReactiveEvent<Vector3> _teleportedEvent;

        private readonly CollidersRegistryService _colllidersRegistryService;

        private IDisposable _teleportedDisposable;

        public ExplodeState(Entity entity, CollidersRegistryService colllidersRegistryService)
        {
            _contactsColliders = entity.ContactCollidersBuffer;
            _contactsEntities = entity.ContactEntitiesBuffer; 
            _body = entity.BodyCollider;
            _mask = entity.ContactsDetectingMask;
            _radius = entity.OnTeleportExplodeRadius;
            _damage = entity.BodyContactDamage;
            _teleportedEvent = entity.TeleportedEvent;

            _colllidersRegistryService = colllidersRegistryService;

            _teleportedDisposable = _teleportedEvent.Subscribe(OnTeleported);
        }

        public void Update(float deltaTime)
        {
        }

        public override void Exit()
        {
            base.Exit();

            _teleportedDisposable.Dispose();
        }

        private void OnTeleported(Vector3 position)
        {
            DetectContacts();

            InitEntitiesFromContacts();

            DealDamageForDetectedEntities();

            _contactsColliders.Clear();
            _contactsEntities.Clear();
        }

        private void DetectContacts()
        {
            _contactsColliders.Count = Physics.OverlapSphereNonAlloc(
                 _body.transform.position,
                 _radius.Value,
                 _contactsColliders.Items,
                 _mask,
                 QueryTriggerInteraction.Ignore);

            _contactsColliders.TryRemove(_body);
        }

        private void InitEntitiesFromContacts()
        {
            _contactsEntities.Count = 0;

            for (int i = 0; i < _contactsColliders.Count; i++)
            {
                Collider collider = _contactsColliders.Items[i];

                Entity contactEntity = _colllidersRegistryService.GetBy(collider);

                if (contactEntity != null)
                {
                    _contactsEntities.Items[_contactsEntities.Count] = contactEntity;
                    _contactsEntities.Count++;
                }
            }
        }

        private void DealDamageForDetectedEntities()
        {
            for (int i = 0; i < _contactsEntities.Count; i++)
            {
                Entity contactEntity = _contactsEntities.Items[i];
                
                if (contactEntity.CanApplyDamage.Evaluate())
                    contactEntity.TakeDamageRequest.Invoke(_damage.Value);
            }
        }
    }
}