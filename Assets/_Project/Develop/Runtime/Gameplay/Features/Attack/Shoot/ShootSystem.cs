using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public abstract class ShootSystem : IInitializableSystem, IDisposableSystem
    {
        protected readonly ProjectilesFactory ProjectilesFactory;
        protected Entity Entity;
        protected Transform ShootPoint;

        private IDisposable _onShootDisposable;

        public ShootSystem(ProjectilesFactory projectilesFactory)
        {
            ProjectilesFactory = projectilesFactory;
        }

        public void OnInit(Entity entity)
        {
            Entity = entity;
            ShootPoint = entity.ShootPoint;

            _onShootDisposable = Entity.StartAttackEvent.Subscribe(Shoot);
        }

        public void OnDispose(Entity entity)
        {
            _onShootDisposable.Dispose();
        }

        protected abstract void Shoot();
    }
}