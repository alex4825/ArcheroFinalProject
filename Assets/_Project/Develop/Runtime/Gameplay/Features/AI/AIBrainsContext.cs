using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class AIBrainsContext : IDisposable, IUpdatable
    {
        private readonly List<EntityToBrain> _entityToBrains = new();

        private bool _isEnabled;

        public void Enable() => _isEnabled = true;
        public void Disable() => _isEnabled = false;

        public void SetFor(Entity entity, IBrain brain)
        {
            foreach (var item in _entityToBrains)
            {
                if (item.Entity == entity)
                {
                    item.Brain.Disable();
                    item.Brain.Dispose();
                    item.Brain = brain;
                    item.Brain.Enable();
                    return;
                }
            }

            _entityToBrains.Add(new EntityToBrain(entity, brain));
            brain.Enable();
        }

        public void Update(float deltaTime)
        {
            if (_isEnabled == false)
                return;

            for (int i = 0; i < _entityToBrains.Count; i++)
            {
                if (_entityToBrains[i].Entity.IsInit == false)
                {
                    int lastIndex = _entityToBrains.Count - 1;

                    _entityToBrains[i].Brain.Dispose();
                    _entityToBrains[i] = _entityToBrains[lastIndex];
                    _entityToBrains.RemoveAt(lastIndex);
                    i--;
                    continue;
                }

                _entityToBrains[i].Brain.Update(deltaTime);
            }
        }

        public void Dispose()
        {
            foreach (EntityToBrain entityToBrain in _entityToBrains)
                entityToBrain.Brain.Dispose();

            _entityToBrains.Clear();
        }

        private class EntityToBrain
        {
            public Entity Entity;
            public IBrain Brain;

            public EntityToBrain(Entity entity, IBrain brain)
            {
                Entity = entity;
                Brain = brain;
            }
        }
    }
}