using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.Shoot
{
    public abstract class ShootSystem : IInitializableSystem, IDisposableSystem
    {
        protected readonly ProjectilesFactory ProjectilesFactory;
        protected Entity Entity;
        protected Transform ShootPoint;

        private IDisposable _attackDelayEndDisposable;

        public ShootSystem(ProjectilesFactory projectilesFactory)
        {
            ProjectilesFactory = projectilesFactory;
        }

        protected abstract ReactiveEvent EventToShoot {  get; }

        public void OnInit(Entity entity)
        {
            Entity = entity;
            ShootPoint = entity.ShootPoint;

            _attackDelayEndDisposable = EventToShoot.Subscribe(OnShoot);
        }

        public void OnDispose(Entity entity)
        {
            _attackDelayEndDisposable.Dispose();
        }

        protected abstract void OnShoot();
    }
}