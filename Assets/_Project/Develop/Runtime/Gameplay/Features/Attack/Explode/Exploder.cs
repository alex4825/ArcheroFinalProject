using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class Exploder
    {
        private Buffer<Collider> _contactsColliders;
        private Buffer<Entity> _contactsEntities;

        private CapsuleCollider _attackerCollider;
        private LayerMask _mask;
        private ReactiveVariable<float> _radius;
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<Teams> _attackerTeam;
        private ReactiveEvent<Vector3> _explodedEvent;

        private readonly CollidersRegistryService _colllidersRegistryService;

        public Exploder(
            CollidersRegistryService colllidersRegistryService,
            ReactiveVariable<float> radius,
            ReactiveVariable<float> damage,
            LayerMask mask,
            ReactiveVariable<Teams> attackerTeam,
            ReactiveEvent<Vector3> explodedEvent = null,
            CapsuleCollider attackerCollider = null)
        {
            _colllidersRegistryService = colllidersRegistryService;

            _radius = radius;
            _damage = damage;
            _mask = mask;
            _attackerCollider = attackerCollider;
            _attackerTeam = attackerTeam;
            _explodedEvent = explodedEvent;

            _contactsColliders = new(64);
            _contactsEntities = new(64);
        }

        public void ExplodeIn(Vector3 point)
        {
            _explodedEvent?.Invoke(point);

            DetectContactsIn(point);

            InitEntitiesFromContacts();

            DealDamageForDetectedEntities();

            _contactsColliders.Clear();
            _contactsEntities.Clear();
        }

        private void DetectContactsIn(Vector3 point)
        {
            _contactsColliders.Count = Physics.OverlapSphereNonAlloc(
                 point,
                 _radius.Value,
                 _contactsColliders.Items,
                 _mask,
                 QueryTriggerInteraction.Ignore);

            if (_attackerCollider != null)
                _contactsColliders.TryRemove(_attackerCollider);
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
                if (EntitiesHelper.TryTakeDamageFrom(_attackerTeam.Value, _contactsEntities.Items[i], _damage.Value))
                    Debug.Log($"Урон нанесён. HP осталось: {_contactsEntities.Items[i].CurrentHealth.Value}");
        }
    }
}