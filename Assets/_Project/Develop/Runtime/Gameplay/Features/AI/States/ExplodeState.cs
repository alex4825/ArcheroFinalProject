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
        private Func<Vector3> _explodePoint;

        private readonly CollidersRegistryService _colllidersRegistryService;

        public ExplodeState(Entity entity, CollidersRegistryService colllidersRegistryService)
        {
            _contactsColliders = entity.ContactCollidersBuffer;
            _contactsEntities = entity.ContactEntitiesBuffer;
            _body = entity.BodyCollider;
            _mask = entity.ContactsDetectingMask;
            _radius = entity.OnTeleportExplodeRadius;
            _damage = entity.BodyContactDamage;
            _explodePoint = () => _body.transform.position;

            _colllidersRegistryService = colllidersRegistryService;
        }

        public ExplodeState(
            CollidersRegistryService colllidersRegistryService,
            Func<Vector3> getPoint,
            float radius,
            float damage,
            LayerMask mask,
            CapsuleCollider selfCollider = null)
        {
            _colllidersRegistryService = colllidersRegistryService;

            _radius = new(radius);
            _damage = new(damage);
            _mask = mask;
            _body = selfCollider;
            _explodePoint = () => getPoint.Invoke();

            _contactsColliders = new(64);
            _contactsEntities = new(64);
        }

        public override void Enter()
        {
            base.Enter();

            DetectContacts();

            InitEntitiesFromContacts();

            DealDamageForDetectedEntities();

            _contactsColliders.Clear();
            _contactsEntities.Clear();
        }

        public void Update(float deltaTime)
        {
        }

        private void DetectContacts()
        {
            _contactsColliders.Count = Physics.OverlapSphereNonAlloc(
                 _explodePoint.Invoke(),
                 _radius.Value,
                 _contactsColliders.Items,
                 _mask,
                 QueryTriggerInteraction.Ignore);

            if (_body != null)
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

                Debug.Log($"Урон нанесён. HP осталось: {contactEntity.CurrentHealth.Value}");
            }
        }
    }
}