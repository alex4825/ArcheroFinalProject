using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Explode
{
    public class ExplodeSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly CollidersRegistryService _collidersRegistryService;

        private Entity _entity;

        public ExplodeSystem(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_entity.MustExplode.Evaluate())
                ExplodeIn(_entity.Transform.position);
        }

        private void ExplodeIn(Vector3 point)
        {
            Exploder exploder = new(
                _collidersRegistryService,
                _entity.ExplodeRadius,
                _entity.ExplodeDamage,
                Layers.EntityMask,
                _entity.Team,
                _entity.ExplodedEvent);

            exploder.ExplodeIn(point);
        }
    }
}