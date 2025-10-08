using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities.Defenders;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastracture.DI;
using System;
using UnityEngine;

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
                    break;

                case CannonConfig cannonConfig:
                    entity = _entitiesFactory.CreateCannon(position, cannonConfig, team);
                    _brainsFactory.CreateCannonBrain(entity);
                    break;

                case TurretConfig turretConfig:
                    entity = _entitiesFactory.CreateTurret(position, turretConfig, team);
                    _brainsFactory.CreateTurretBrain(entity);
                    break;

                default:
                    throw new ArgumentException($"Not support {config.GetType()} type config");
            }

            _entitiesLifeContext.Add(entity);

            return entity;
        }
    }
}