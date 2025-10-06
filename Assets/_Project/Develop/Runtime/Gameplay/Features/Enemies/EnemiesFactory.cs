using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using UnityEngine;
using System;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Enemies
{
    public class EnemiesFactory
    {
        private readonly DIContainer _container;
        private readonly EntitiesFactory _entitiesFactory;
        private readonly BrainsFactory _brainsFactory;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public EnemiesFactory(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public Entity Create(Vector3 position, EntityConfig config, Teams team)
        {
            Entity entity;

            switch (config)
            {
                case GhostConfig ghostConfig:
                    entity = _entitiesFactory.CreateGhost(position, ghostConfig, team);
                    _brainsFactory.CreateGhostBrain(entity);
                    break;

                case ExplodyConfig explodyConfig:
                    entity = _entitiesFactory.CreateExplody(position, explodyConfig, team);
                    _brainsFactory.CreateExplodyBrain(entity);
                    break;

                case MineConfig mineConfig:
                    entity = _entitiesFactory.CreateMine(position, mineConfig);
                    _brainsFactory.CreateMineBrain(entity);
                    break;

                case FlamingPoolConfig flamingPoolConfig:
                    entity = _entitiesFactory.CreateFlamingPool(position, flamingPoolConfig);
                    //_brainsFactory.CreateMineBrain(entity);
                    break;

                case CannonConfig cannonConfig:
                    entity = _entitiesFactory.CreateCannon(position, cannonConfig, team);
                    _brainsFactory.CreateCannonBrain(entity);
                    break;

                default:
                    throw new ArgumentException($"Not support {config.GetType()} type config");
            }

            _entitiesLifeContext.Add(entity);

            return entity;
        }
    }
}