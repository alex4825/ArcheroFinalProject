using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage
{
    public class DealDamageOnContactPerTimeSystem : IInitializableSystem, IUpdatableSystem
    {
        private Entity _entity;
        private Buffer<Entity> _contacts;
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _timeToDealDamage;

        public DealDamageOnContactPerTimeSystem(ReactiveVariable<float> damage)
        {
            _damage = damage;
        }

        private Dictionary<Entity, float> _entityToPassedTime;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _contacts = entity.ContactEntitiesBuffer;
            _timeToDealDamage = entity.TimeToDealDamage;

            _entityToPassedTime = new Dictionary<Entity, float>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_contacts.Count <= 0)
                return;

            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contactEntity = _contacts.Items[i];

                if (_entityToPassedTime.ContainsKey(contactEntity))
                {
                    _entityToPassedTime[contactEntity] += deltaTime;

                    if (_entityToPassedTime[contactEntity] >= _timeToDealDamage.Value)
                    {
                        EntitiesHelper.TryTakeDamageFrom(_entity, contactEntity, _damage.Value);
                        _entityToPassedTime[contactEntity] = 0;
                    }
                }
                else
                {
                    _entityToPassedTime.Add(contactEntity, _timeToDealDamage.Value);
                }
            }

            DeleteNeedlessEntities();
        }

        private void DeleteNeedlessEntities()
        {
            List<Entity> entitiesToRemove = new List<Entity>();

            foreach (Entity entity in _entityToPassedTime.Keys)
                if (_contacts.Items.Contains(entity) == false)
                    entitiesToRemove.Add(entity);

            foreach (Entity entity in entitiesToRemove)
                _entityToPassedTime.Remove(entity);
        }
    }
}
